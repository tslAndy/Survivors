using System.Numerics;
using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.Relationships;
using Arch.System;
using Autofac;
using Components.Basic;
using Components.Characters;
using Components.Fighting;
using Components.Loot;
using Components.Other;
using Components.Physics;
using Engine;
using Engine.Animations;
using Engine.Common;
using Engine.Tilemaps;
using Raylib_cs;
using Systems;
using Utils;
using Weapons;
using Weapons.Specific;

class Game : IDisposable
{
    private readonly ILifetimeScope _scope;

    private readonly World _world;
    private readonly CommandBuffer _commandBuffer;
    private readonly Group<float> _systems;

    public Game()
    {
        ContainerBuilder builder = new ContainerBuilder();
        builder.RegisterModule(new EngineModule());
        builder.RegisterModule(new SystemsModule());

        IContainer container = builder.Build();
        _scope = container.BeginLifetimeScope();

        _world = _scope.Resolve<World>();
        _commandBuffer = _scope.Resolve<CommandBuffer>();
        _systems = _scope.Resolve<Group<float>>();

        LoadTilemaps();
        CreateEnemies();
        CreatePlayer();
    }

    private float t;

    public void Update()
    {
        _systems.Update(Raylib.GetFrameTime());
        _commandBuffer.Playback(_world, true);

        t += Raylib.GetFrameTime();
        if (t > 2.0f)
        {
            t = 0.0f;
            CreateEnemies();
        }
    }

    private void LoadTilemaps()
    {
        Tilemap floor = _scope
            .Resolve<TilemapManager>()
            .Get("./Resources/Tilemaps/Level_A/Level_A_Floor.tilemap");

        _world.Create<TilemapComp>(new TilemapComp { tilemap = floor, drawOrder = 0 });

        Tilemap walls = _scope
            .Resolve<TilemapManager>()
            .Get("./Resources/Tilemaps/Level_A/Level_A_Walls.tilemap");

        int tilemapLayer = _scope.Resolve<LayerMap>()["Walls"];

        _world.Create<TilemapComp, RigidComp>(
            new TilemapComp { tilemap = walls, drawOrder = 1 },
            new RigidComp { layer = tilemapLayer }
        );
    }

    private void CreateEnemies()
    {
        int enemyLayer = _scope.Resolve<LayerMap>()["EnemyEnts"];

        AnimAtlas goblinAtlas = _scope
            .Resolve<AnimAtlasManager>()
            .Get("./Resources/AnimAtlases/Entities/Goblin.animAtlas");
        for (int i = 0; i < 30; i++)
        {
            Entity enemy = _world.Create<
                CharMoveComp,
                SpriteComp,
                AnimComp,
                TransformComp,
                RigidComp,
                CollComp,
                HealthComp,
                DamageComp,
                StatusEffectComp,
                DropComp
            >(
                new CharMoveComp { maxSpeed = 3.0f, speedFactor = 1.0f },
                new SpriteComp { drawOrder = 1 },
                new AnimComp
                {
                    atlas = goblinAtlas,
                    anim = goblinAtlas["Idle_Down"],
                    animDir = AnimDir.Down,
                    timeScale = 1.0f,
                },
                new TransformComp
                {
                    position = new Vector2(
                        2.0f + Random.Shared.NextSingle() * 26.0f,
                        2.0f + Random.Shared.NextSingle() * 16.0f
                    ),
                    scale = 1.0f,
                },
                new RigidComp { layer = enemyLayer },
                new CollComp { radius = 0.5f },
                new HealthComp { currentHP = 100, maxHP = 100 },
                new DamageComp { hits = CachedList<Hit>.Create(), damageFactor = 1.0f },
                new StatusEffectComp
                {
                    newEffects = CachedList<StatusEffect>.Create(),
                    runningEffects = CachedList<StatusEffect>.Create(),
                },
                new DropComp { amount = Random.Shared.Next(1, 5000) }
            );
        }
    }

    private void CreatePlayer()
    {
        AnimAtlas playerAnimAtlas = _scope
            .Resolve<AnimAtlasManager>()
            .Get("./Resources/AnimAtlases/Entities/Player.animAtlas");

        CachedList<WeaponElem> weapons = CachedList<WeaponElem>.Create();

        WeaponConfig weaponConfig = new WeaponConfig
        {
            baseDamage = 10,
            critDamage = 10,
            critChance = 30,
            attackTime = 0.75f,
            detectRadius = 4.0f,
            targetLayer = _scope.Resolve<LayerMap>()["EnemyEnts"],
        };

        WeaponCallbacks callbacks = new WeaponCallbacks
        {
            onBaseDamage = (attacker, target, ref damage) =>
            {
                attacker
                    .Get<StatusEffectComp>()
                    .newEffects.Add(new StatusEffect(StatusEffectType.Rage, 10.0f, 3.0f));

                attacker
                    .Get<StatusEffectComp>()
                    .newEffects.Add(new StatusEffect(StatusEffectType.Haste, 10.0f, 5.0f));
                //
                // target
                //     .Get<StatusEffectComp>()
                //     .newEffects.Add(new StatusEffect(StatusEffectType.Sensitivity, 3.0f, 3.0f));
            },
        };

        AnimAtlas swingAtlas = _scope
            .Resolve<AnimAtlasManager>()
            .Get("./Resources/AnimAtlases/Items/BattleEffects.animAtlas");
        Entity swing = _world.Create<TransformComp, SpriteComp, AnimComp>(
            new TransformComp(),
            new SpriteComp { drawOrder = 2 },
            new AnimComp
            {
                anim = swingAtlas["Swing_A"],
                atlas = swingAtlas,
                timeScale = 1.0f,
            }
        );

        Weapon weapon = new MeleeWeapon(weaponConfig, callbacks, _scope.Resolve<WorldContext>());
        weapons.Add(new WeaponElem(weapon, swing));

        CachedList<ShieldElem> shields = CachedList<ShieldElem>.Create();

        Entity player = _world.Create<
            PlayerComp,
            CharMoveComp,
            SpriteComp,
            AnimComp,
            TransformComp,
            RigidComp,
            CollComp,
            ShieldComp,
            WeaponComp,
            DamageComp,
            HealthComp,
            StatusEffectComp,
            LootCollComp
        >(
            new PlayerComp { state = PlayerState.Idle },
            new CharMoveComp { maxSpeed = 3.0f, speedFactor = 1.0f },
            new SpriteComp { drawOrder = 1 },
            new AnimComp
            {
                atlas = playerAnimAtlas,
                anim = playerAnimAtlas["Idle_Up"],
                animDir = AnimDir.Up,
                timeScale = 1.0f,
            },
            new TransformComp { position = new Vector2(15.0f, 10.0f), scale = 1.0f },
            new RigidComp { layer = _scope.Resolve<LayerMap>()["PlayerEnts"] },
            new CollComp { radius = 0.5f },
            new ShieldComp { shields = shields, dpsFactor = 1.0f },
            new WeaponComp { weapons = weapons, dpsFactor = 1.0f },
            new DamageComp { hits = CachedList<Hit>.Create() },
            new HealthComp { currentHP = 100, maxHP = 100 },
            new StatusEffectComp
            {
                newEffects = CachedList<StatusEffect>.Create(),
                runningEffects = CachedList<StatusEffect>.Create(),
            },
            new LootCollComp
            {
                radius = 15.0f,
                speed = 10.0f,
                incomeFactor = 1.0f,
                radiusFactor = 1.0f,
            }
        );

        player.AddRelationship<TrsOwn>(swing);
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
