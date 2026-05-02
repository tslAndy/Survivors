using System.Numerics;
using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Autofac;
using Components.Basic;
using Components.Fighting;
using Components.Loot;
using Components.Other;
using Components.Physics;
using Components.Player;
using Engine;
using Engine.Animations;
using Engine.Common;
using Engine.Sprites;
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
        builder.RegisterModule(new WeaponsModule());

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
        for (int i = 0; i < 1; i++)
        {
            _world.Create<
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
                new SpriteComp { drawOrder = 1 },
                new AnimComp
                {
                    atlas = goblinAtlas,
                    anim = goblinAtlas["IdleDown"],
                    animDir = AnimDir.Down,
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
                new DamageComp { hits = CachedList<Hit>.Create() },
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

        CachedList<Weapon> weapons = CachedList<Weapon>.Create();

        WeaponConfig weaponConfig = new WeaponConfig
        {
            baseDamage = 10,
            critDamage = 15,
            critChance = 30,
            attackTime = 1.0f,
            detectRadius = 10.0f,
            targetLayer = _scope.Resolve<LayerMap>()["EnemyEnts"],
        };

        BulletConfig bulletConfig = new BulletConfig
        {
            radius = 0.125f,
            speed = 15.0f,
            layer = _scope.Resolve<LayerMap>()["PlayerBullets"],
            sprite = _scope
                .Resolve<SpriteAtlasManager>()
                .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")["arrow_1"],
        };

        WeaponCallbacks callbacks = new WeaponCallbacks
        {
            onBaseDamage = (Entity attacker, Entity target, ref float baseDamage) =>
            {
                Console.WriteLine("hehehe");
                target
                    .Get<StatusEffectComp>()
                    .newEffects.Add(new StatusEffect(StatusEffectType.Burn, 10.0f, 5.0f));
            },
        };
        Bow bow = _scope.Resolve<Bow>(
            new TypedParameter(typeof(BulletConfig), bulletConfig),
            new TypedParameter(typeof(WeaponConfig), weaponConfig),
            new TypedParameter(typeof(WeaponCallbacks), callbacks)
        );

        weapons.Add(bow);

        Entity player = _world.Create<
            PlayerComp,
            SpriteComp,
            AnimComp,
            TransformComp,
            RigidComp,
            CollComp,
            WeaponComp,
            LootCollComp
        >(
            new PlayerComp
            {
                walkSpeed = 2.0f,
                runSpeed = 3.0f,
                state = PlayerState.Idle,
            },
            new SpriteComp { drawOrder = 1 },
            new AnimComp
            {
                atlas = playerAnimAtlas,
                anim = playerAnimAtlas["IdleUp"],
                animDir = AnimDir.Up,
            },
            new TransformComp { position = new Vector2(15.0f, 10.0f), scale = 1.0f },
            new RigidComp { layer = _scope.Resolve<LayerMap>()["PlayerEnts"] },
            new CollComp { radius = 0.5f },
            new WeaponComp { weapons = weapons },
            new LootCollComp { radius = 15.0f, speed = 10.0f }
        );
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
