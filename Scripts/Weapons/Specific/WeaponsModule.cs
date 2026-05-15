using Arch.Core;
using Arch.Core.Extensions;
using Autofac;
using Components.Basic;
using Components.Fighting;
using Components.Health;
using Engine.Animations;
using Engine.Common;
using Engine.Sounds;
using Engine.Sprites;
using Raylib_cs;
using Systems;
using Utils;

namespace Weapons.Specific;

class WeaponsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register<WeaponElem>(x =>
            {
                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 1,
                    critDamage = 2,
                    critChance = 30,
                    attackTime = 0.0f,
                    detectRadius = 4.0f,
                    targetLayer = x.Resolve<LayerMap>()["PlayerEnts"],
                    maxEnemies = 10,
                };

                BulletConfig bulletConfig = new BulletConfig
                {
                    sprite = x.Resolve<SpriteAtlasManager>()
                        .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")["arrow_2"],
                    velocity = 8.0f,
                    radius = 0.25f,
                    lifetime = 4.0f,
                    bulletLayer = x.Resolve<LayerMap>()["EnemyBullets"],
                    drawOrder = 2,
                    perforate = false,
                    bounce = false,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                IWeapon weapon = new Bow(
                    bulletConfig,
                    config,
                    callbacks,
                    x.Resolve<WorldContext>()
                );
                return new WeaponElem(weapon, null);
            })
            .Named<WeaponElem>("goblinBow")
            .InstancePerDependency();

        builder
            .Register<WeaponElem>(x =>
            {
                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 20,
                    critDamage = 50,
                    critChance = 30,
                    attackTime = 1.5f,
                    detectRadius = 4.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    maxEnemies = 1,
                };

                BulletConfig bulletConfig = new BulletConfig
                {
                    sprite = x.Resolve<SpriteAtlasManager>()
                        .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")[
                        "boomerang_1"
                    ],
                    velocity = 8.0f,
                    radius = 0.5f,
                    lifetime = 10.0f,
                    bulletLayer = x.Resolve<LayerMap>()["PlayerBullets"],
                    drawOrder = 2,
                    perforate = true,
                    bounce = true,
                };

                BoomerangConfig boomrConfig = new BoomerangConfig
                {
                    maxDist = 10.0f,
                    rotSpeed = 180.0f,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                IWeapon weapon = new Boomerang(
                    boomrConfig,
                    bulletConfig,
                    config,
                    callbacks,
                    x.Resolve<WorldContext>()
                );
                return new WeaponElem(weapon, null);
            })
            .Named<WeaponElem>("simpleBoomerang")
            .InstancePerDependency();

        builder
            .Register<WeaponElem>(x =>
            {
                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 20,
                    critDamage = 50,
                    critChance = 30,
                    attackTime = 0.5f,
                    detectRadius = 4.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    maxEnemies = 10,
                };

                BulletConfig bulletConfig = new BulletConfig
                {
                    sprite = x.Resolve<SpriteAtlasManager>()
                        .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")["arrow_1"],
                    velocity = 8.0f,
                    radius = 0.25f,
                    lifetime = 4.0f,
                    bulletLayer = x.Resolve<LayerMap>()["PlayerBullets"],
                    drawOrder = 2,
                    perforate = false,
                    bounce = false,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                IWeapon weapon = new Bow(
                    bulletConfig,
                    config,
                    callbacks,
                    x.Resolve<WorldContext>()
                );
                return new WeaponElem(weapon, null);
            })
            .Named<WeaponElem>("simpleBow")
            .InstancePerDependency();

        builder
            .Register<WeaponElem>(x =>
            {
                TrailConfig trailConfig = new TrailConfig
                {
                    length = 5.0f,
                    thick = 0.1f,
                    color = Color.Green,
                };

                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 20,
                    critDamage = 50,
                    critChance = 30,
                    attackTime = 0.5f,
                    detectRadius = 2.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    maxEnemies = 10,
                };

                BulletConfig bulletConfig = new BulletConfig
                {
                    velocity = 4.0f,
                    radius = 0.01f,
                    lifetime = 90.0f,
                    bulletLayer = x.Resolve<LayerMap>()["PlayerBullets"],
                    drawOrder = 2,
                    perforate = true,
                    bounce = true,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                IWeapon weapon = new TrailBow(
                    trailConfig,
                    bulletConfig,
                    config,
                    callbacks,
                    x.Resolve<WorldContext>()
                );

                Entity extra = x.Resolve<World>()
                    .Create<LineComp, TrsComp, LocalTrsComp>(
                        new LineComp { lines = CachedList<Line>.Create() }
                    );
                return new WeaponElem(weapon, extra);
            })
            .Named<WeaponElem>("simpleTrailBow")
            .InstancePerDependency();

        builder
            .Register<WeaponElem>(x =>
            {
                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 20,
                    critDamage = 50,
                    critChance = 30,
                    attackTime = 0.75f,
                    detectRadius = 2.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    maxEnemies = 10,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                AnimAtlas swingAtlas = x.Resolve<AnimAtlasManager>()
                    .Get("./Resources/AnimAtlases/Items/BattleEffects.animAtlas");
                //
                Entity swing = x.Resolve<World>()
                    .Create<TrsComp, LocalTrsComp, SpriteComp, AnimComp, ModComp>(
                        new TrsComp { scale = 1.0f },
                        new LocalTrsComp { scale = 1.0f },
                        new SpriteComp { drawOrder = 2 },
                        new AnimComp { anim = swingAtlas["Swing_1"], atlas = swingAtlas },
                        new ModComp()
                    );

                IWeapon weapon = new MeleeWeapon(config, callbacks, x.Resolve<WorldContext>());
                return new WeaponElem(weapon, swing);
            })
            .Named<WeaponElem>("simpleSword")
            .InstancePerDependency();

        builder
            .Register<WeaponElem>(x =>
            {
                BulletConfig bulletConfig = new BulletConfig
                {
                    sprite = x.Resolve<SpriteAtlasManager>()
                        .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")["chakram_1"],
                    radius = 0.25f,
                    lifetime = float.MaxValue,
                    bulletLayer = x.Resolve<LayerMap>()["PlayerBullets"],
                    drawOrder = 2,
                };

                SpinConfig spinConfig = new SpinConfig
                {
                    rotSpeed = 90.0f,
                    circleRadius = 2.5f,
                    bulletsCount = 10,
                };

                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 10,
                    critDamage = 20,
                    critChance = 30,
                    attackTime = 0.75f,
                    detectRadius = 2.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    maxEnemies = 10,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks
                {
                    onBaseDamage = (attacker, target, ref val) =>
                    {
                        attacker
                            .Get<StatusEffectComp>()
                            .newEffects.Add(
                                new StatusEffect(StatusEffectType.AttackFast, 10.0f, 3.0f)
                            );
                    },
                };

                IWeapon weapon = new SpinWeapon(
                    spinConfig,
                    bulletConfig,
                    config,
                    callbacks,
                    x.Resolve<WorldContext>()
                );

                Entity center = x.Resolve<World>()
                    .Create<TrsComp, LocalTrsComp>(
                        new TrsComp { scale = 1.0f, descs = CachedList<Entity>.Create() },
                        new LocalTrsComp { scale = 1.0f }
                    );

                return new WeaponElem(weapon, center);
            })
            .Named<WeaponElem>("simpleSpin")
            .InstancePerDependency();

        builder
            .Register<WeaponElem>(x =>
            {
                LaserConfig laserConfig = new LaserConfig
                {
                    raysCount = 10,
                    start = 0.5f,
                    end = 10.0f,
                    thick = 0.3f,
                    rotSpeed = 40,
                    color = Color.Red,
                };

                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 1,
                    critDamage = 2,
                    critChance = 30,
                    attackTime = 0.1f,
                    detectRadius = 2.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    maxEnemies = 10,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks
                {
                    onBaseDamage = (attacker, target, ref val) =>
                    {
                        attacker
                            .Get<StatusEffectComp>()
                            .newEffects.Add(
                                new StatusEffect(StatusEffectType.AttackFast, 10.0f, 3.0f)
                            );
                    },
                };

                IWeapon weapon = new LaserWeapon(
                    laserConfig,
                    config,
                    callbacks,
                    x.Resolve<WorldContext>()
                );

                Entity extra = x.Resolve<World>()
                    .Create<LineComp, TrsComp, LocalTrsComp>(
                        new LineComp { lines = CachedList<Line>.Create() }
                    );

                return new WeaponElem(weapon, extra);
            })
            .Named<WeaponElem>("simpleLaser")
            .InstancePerDependency();
    }
}
