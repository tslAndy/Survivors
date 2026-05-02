using System.Numerics;
using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Components.Loot;
using Components.Other;
using Components.Physics;
using Components.Player;
using Engine.Animations;
using Engine.Common;
using Engine.Sounds;
using Engine.Sprites;
using Engine.Tilemaps;
using Raylib_cs;
using Systems.Animation;
using Systems.Drawing;
using Systems.Fighting;
using Systems.Physics;
using Systems.Player;
using Utils;
using Weapons;
using Weapons.Specific;

class Game : IDisposable
{
    private readonly World _world;
    private readonly CommandBuffer _commBuffer;
    private readonly Group<float> _systems;

    private Camera testCamera;

    public Game()
    {
        ServiceLocator.Register(new TextureManager());
        ServiceLocator.Register(new SpriteAtlasManager());
        ServiceLocator.Register(new SoundManager());
        ServiceLocator.Register(new SoundAtlasManager());
        ServiceLocator.Register(new AnimAtlasManager());
        ServiceLocator.Register(new TilesetManager());
        ServiceLocator.Register(new TilemapManager());
        ServiceLocator.Register(new LayerCollMap());
        ServiceLocator.Register<Camera>(new Camera(1280, 720, new Vector2(15.0f, 10.0f), 30.0f));

        // INIT WORLD
        _world = World.Create();
        ServiceLocator.Register<World>(_world);

        _commBuffer = new CommandBuffer();
        ServiceLocator.Register<CommandBuffer>(_commBuffer);

        SpatialSys spatialSys = new SpatialSys(_world);
        ServiceLocator.Register(spatialSys);

        TileCollSys tileCollSys = new TileCollSys(_world);
        ServiceLocator.Register(tileCollSys);

        PlayerSys playerSys = new PlayerSys(_world);
        ServiceLocator.Register(playerSys);

        BulletSys bulletSys = new BulletSys(_world);
        ServiceLocator.Register(bulletSys);

        WeaponSys weaponSys = new WeaponSys(_world);
        ServiceLocator.Register(weaponSys);

        ShieldSys shieldSys = new ShieldSys(_world);
        ServiceLocator.Register(shieldSys);

        StatusEffectSys effectSys = new StatusEffectSys(_world);
        ServiceLocator.Register(effectSys);

        DamageSys damageSys = new DamageSys(_world);
        ServiceLocator.Register(damageSys);

        HealthSys healthSys = new HealthSys(_world);
        ServiceLocator.Register(healthSys);

        DropSys dropSys = new DropSys(_world);
        ServiceLocator.Register(dropSys);

        LootCollSys lootCollSys = new LootCollSys(_world);
        ServiceLocator.Register(lootCollSys);

        DeathSys deathSys = new DeathSys(_world);
        ServiceLocator.Register(deathSys);

        RigidSys rigidSys = new RigidSys(_world);
        ServiceLocator.Register(rigidSys);

        SoundSys soundSys = new SoundSys(_world);
        ServiceLocator.Register(soundSys);

        SpriteDrawSys spriteDrawSys = new SpriteDrawSys(_world);
        ServiceLocator.Register(spriteDrawSys);

        AnimSys animSys = new AnimSys(_world);
        ServiceLocator.Register(animSys);

        TilemapDrawSys tilemapDrawSys = new TilemapDrawSys(_world);
        ServiceLocator.Register(tilemapDrawSys);

        TextDrawSys textDrawSys = new TextDrawSys(_world);
        ServiceLocator.Register(textDrawSys);

        // INIT SYSTEMS
        _systems = new Group<float>(
            "Base Systems",
            //physics
            spatialSys,
            tileCollSys,
            rigidSys,
            // player systems
            playerSys,
            // fighting
            bulletSys,
            weaponSys,
            shieldSys,
            effectSys,
            damageSys,
            healthSys,
            // loot
            dropSys,
            lootCollSys,
            // death
            deathSys,
            // audio and drawing
            animSys,
            tilemapDrawSys,
            spriteDrawSys,
            textDrawSys,
            soundSys
        );
        _systems.Initialize();

        testCamera = ServiceLocator.Get<Camera>();
        Test();
    }

    // TODO: remove
    private float time = 0.0f;

    public void Update()
    {
        testCamera.size += Raylib.GetMouseWheelMove() * 5.0f;

        _systems.Update(Raylib.GetFrameTime());
        _commBuffer.Playback(_world, true);

        time += Raylib.GetFrameTime();
        if (time < 2.0f)
            return;
        time = 0.0f;

        AnimAtlas goblinAtlas = ServiceLocator
            .Get<AnimAtlasManager>()
            .Get("./Resources/AnimAtlases/Entities/Goblin.animAtlas");
        for (int i = 0; i < 20; i++)
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
                new RigidComp { layer = (int)Layers.EnemiesEnts },
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

    private void Test()
    {
        // MAP SETUP
        Tilemap walls = ServiceLocator
            .Get<TilemapManager>()
            .Get("./Resources/Tilemaps/Level_A/Level_A_Walls.tilemap");

        Tilemap floor = ServiceLocator
            .Get<TilemapManager>()
            .Get("./Resources/Tilemaps/Level_A/Level_A_Floor.tilemap");

        _world.Create<TilemapComp, RigidComp>(
            new TilemapComp { tilemap = walls, drawOrder = 1 },
            new RigidComp { layer = default }
        );
        _world.Create<TilemapComp>(new TilemapComp { tilemap = floor, drawOrder = 0 });

        // PLAYER SETUP
        AnimAtlas playerAnimAtlas = ServiceLocator
            .Get<AnimAtlasManager>()
            .Get("./Resources/AnimAtlases/Entities/Player.animAtlas");

        CachedList<Weapon> weapons = CachedList<Weapon>.Create();
        WeaponConfig weaponConfig = new WeaponConfig
        {
            baseDamage = 10,
            critDamage = 15,
            critChance = 30,
            attackTime = 1.0f,
            detectRadius = 10.0f,
        };

        WeaponCallbacks callbacks = new WeaponCallbacks { };

        weapons.Add(
            new Bow(
                new BulletConfig
                {
                    radius = 0.125f,
                    speed = 15.0f,
                    layer = (int)Layers.PlayerBullets,
                    sprite = ServiceLocator
                        .Get<SpriteAtlasManager>()
                        .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")["arrow_1"],
                },
                weaponConfig,
                callbacks,
                (int)Layers.EnemiesEnts
            )
        );

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
            new RigidComp { layer = (int)Layers.PlayerEnts },
            new CollComp { radius = 0.5f },
            new WeaponComp { weapons = weapons },
            new LootCollComp { radius = 15.0f, speed = 10.0f }
        );
    }

    public void Dispose()
    {
        ServiceLocator.Dispose();
    }
}

enum Layers
{
    PlayerEnts,
    EnemiesEnts,
    PlayerBullets,
    EnemiesBullets,
    Loot,
}
