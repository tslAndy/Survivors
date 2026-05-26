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
using Other;
using Raylib_cs;
using Systems;
using Utils;
using Weapons.Specific;

namespace Weapons;

public class WeaponsModule : Module
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
                    detectRadius = 1.0f,
                    targetLayer = x.Resolve<LayerMap>()["PlayerEnts"],
                    maxEnemies = 10,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                IWeapon weapon = new MeleeWeapon(config, callbacks, x.Resolve<WorldContext>());
                return new WeaponElem(weapon, null);
            })
            .Named<WeaponElem>("goblinSword")
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
                    rotVelocity = 180.0f,
                    radius = 0.5f,
                    lifetime = 10.0f,
                    bulletLayer = x.Resolve<LayerMap>()["PlayerBullets"],
                    drawOrder = 2,
                    perforate = true,
                    bounce = true,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                IWeapon weapon = new Boomerang(
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
                    attackTime = 1.5f,
                    detectRadius = 2.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    maxEnemies = 10,
                };

                BulletConfig bulletConfig = new BulletConfig
                {
                    velocity = 8.0f,
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

                CachedList<Line> lines = CachedList<Line>.Create();
                Entity extra = x.Resolve<World>()
                    .Create<LineComp, DispComp, TrsComp, LocalTrsComp>(
                        new LineComp { lines = lines },
                        new DispComp(lines)
                    );
                return new WeaponElem(weapon, extra);
            })
            .Named<WeaponElem>("simpleTrailBow")
            .InstancePerDependency();

        builder
            .Register<WeaponElem>(x =>
            {
                Anim anim = x.Resolve<AnimAtlasManager>()
                    .Get("./Resources/AnimAtlases/Items/BattleEffects.animAtlas")["Swing_1"];

                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 20,
                    critDamage = 50,
                    critChance = 30,
                    attackTime = anim.duration,
                    detectRadius = 3.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    maxEnemies = 10,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                Entity swing = x.Resolve<World>()
                    .Create<TrsComp, LocalTrsComp, SpriteComp, AnimComp, ModComp>(
                        new TrsComp { scale = 1.0f },
                        new LocalTrsComp { scale = 1.0f },
                        new SpriteComp { drawOrder = 2 },
                        new AnimComp { anim = anim },
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
                    rotVelocity = 360.0f,
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

                CachedList<Entity> descs = CachedList<Entity>.Create();
                Entity center = x.Resolve<World>()
                    .Create<TrsComp, LocalTrsComp, DispComp>(
                        new TrsComp { scale = 1.0f, descs = descs },
                        new LocalTrsComp { scale = 1.0f },
                        new DispComp(descs)
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
                    thick = 0.1f,
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

                CachedList<Line> lines = CachedList<Line>.Create();
                Entity extra = x.Resolve<World>()
                    .Create<LineComp, DispComp, TrsComp, LocalTrsComp>(
                        new LineComp { lines = lines },
                        new DispComp(lines)
                    );

                return new WeaponElem(weapon, extra);
            })
            .Named<WeaponElem>("simpleLaser")
            .InstancePerDependency();

        builder
            .Register<WeaponElem>(x =>
            {
                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 20,
                    critDamage = 50,
                    critChance = 30,
                    attackTime = 1.0f,
                    detectRadius = 4.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    maxEnemies = 10,
                };

                BulletConfig bulletConfig = new BulletConfig
                {
                    sprite = x.Resolve<SpriteAtlasManager>()
                        .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")["kunai_1"],
                    velocity = 8.0f,
                    radius = 0.25f,
                    lifetime = 4.0f,
                    bulletLayer = x.Resolve<LayerMap>()["PlayerBullets"],
                    drawOrder = 2,
                    perforate = true,
                    bounce = false,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                IWeapon weapon = new Kunai(
                    bulletConfig,
                    config,
                    callbacks,
                    x.Resolve<WorldContext>()
                );
                return new WeaponElem(weapon, null);
            })
            .Named<WeaponElem>("simpleKunai")
            .InstancePerDependency();

        builder
            .Register<WeaponElem>(x =>
            {
                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 20,
                    critDamage = 50,
                    critChance = 30,
                    attackTime = 1.0f,
                    detectRadius = 4.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    maxEnemies = 10,
                };

                Anim anim = x.Resolve<AnimAtlasManager>()
                    .Get("./Resources/AnimAtlases/Items/BattleEffects.animAtlas")["Explosion_1"];

                BulletConfig bulletConfig = new BulletConfig
                {
                    anim = anim,
                    radius = 1.0f,
                    lifetime = anim.duration,
                    bulletLayer = x.Resolve<LayerMap>()["PlayerBullets"],
                    drawOrder = 2,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                IWeapon weapon = new Book(
                    bulletConfig,
                    config,
                    callbacks,
                    x.Resolve<WorldContext>()
                );
                return new WeaponElem(weapon, null);
            })
            .Named<WeaponElem>("simpleBook")
            .InstancePerDependency();

        builder
            .Register<WeaponElem>(x =>
            {
                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 20,
                    critDamage = 50,
                    critChance = 30,
                    attackTime = 2.0f,
                    detectRadius = 8.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    maxEnemies = 50,
                };

                Anim anim = x.Resolve<AnimAtlasManager>()
                    .Get("./Resources/AnimAtlases/Items/BattleEffects.animAtlas")["Cast_5"];

                BulletConfig bulletConfig = new BulletConfig
                {
                    anim = anim,
                    radius = 1.0f,
                    lifetime = anim.duration,
                    bulletLayer = x.Resolve<LayerMap>()["PlayerBullets"],
                    drawOrder = 2,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                IWeapon weapon = new Card(
                    bulletConfig,
                    config,
                    callbacks,
                    x.Resolve<WorldContext>()
                );
                return new WeaponElem(weapon, null);
            })
            .Named<WeaponElem>("simpleCard")
            .InstancePerDependency();

        builder
            .Register<ShieldElem>(x =>
            {
                ShieldConfig config = new ShieldConfig
                {
                    detectRadius = 2.0f,
                    entityLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    projectileLayer = x.Resolve<LayerMap>()["EnemyBullets"],
                };

                ShieldCallbacks callbacks = new ShieldCallbacks
                {
                    hitCallback = (ent, ref hit) =>
                    {
                        if (hit.source == null)
                            return;
                        ref DamageComp damage = ref hit.source.Value.Get<DamageComp>();
                        damage.hits.Add(new Hit(1_000_000, StatusEffectType.None));
                    },
                };

                IShield shield = new Shield(config, callbacks, x.Resolve<WorldContext>());
                return new ShieldElem(shield, null);
            })
            .Named<ShieldElem>("curseShield")
            .InstancePerDependency();

        builder
            .Register<ShieldElem>(x =>
            {
                ShieldConfig config = new ShieldConfig
                {
                    detectRadius = 2.0f,
                    entityLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    projectileLayer = x.Resolve<LayerMap>()["EnemyBullets"],
                };

                IShield shield = new DestroyShield(config, default, x.Resolve<WorldContext>());
                return new ShieldElem(shield, null);
            })
            .Named<ShieldElem>("destroyShield")
            .InstancePerDependency();

        builder
            .Register<ShieldElem>(x =>
            {
                WeaponConfig weaponConfig = new WeaponConfig
                {
                    baseDamage = 20,
                    critDamage = 50,
                    critChance = 30,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                };

                BulletConfig bulletConfig = new BulletConfig { perforate = false, bounce = false };

                IBulletWeapon weapon = new Bow(
                    bulletConfig,
                    weaponConfig,
                    default,
                    x.Resolve<WorldContext>()
                );

                ShieldConfig shieldConfig = new ShieldConfig
                {
                    detectRadius = 1.0f,
                    entityLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    projectileLayer = x.Resolve<LayerMap>()["EnemyBullets"],
                };

                IShield shield = new ReflectShield(
                    weapon,
                    shieldConfig,
                    default,
                    x.Resolve<WorldContext>()
                );

                return new ShieldElem(shield, null);
            })
            .Named<ShieldElem>("reflectShield")
            .InstancePerDependency();

        builder
            .Register<WeaponElem>(x =>
            {
                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 20,
                    critDamage = 50,
                    critChance = 30,
                    attackTime = 1.0f,
                    detectRadius = 4.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                    maxEnemies = 10,
                };

                BulletConfig bulletConfig = new BulletConfig
                {
                    anim = x.Resolve<AnimAtlasManager>()
                        .Get("./Resources/AnimAtlases/Items/BattleEffects.animAtlas")["Fireball_5"],
                    velocity = 8.0f,
                    radius = 0.25f,
                    lifetime = 4.0f,
                    bulletLayer = x.Resolve<LayerMap>()["PlayerBullets"],
                    drawOrder = 2,
                    perforate = true,
                    bounce = false,
                };

                IWeapon weapon = new Stave(
                    bulletConfig,
                    config,
                    default,
                    x.Resolve<WorldContext>()
                );
                return new WeaponElem(weapon, null);
            })
            .Named<WeaponElem>("simpleStave")
            .InstancePerDependency();
    }
}

class ItemsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register<Item>(x =>
            {
                return new WeaponItem(
                    new ItemInfo(
                        "simpleBow",
                        x.Resolve<SpriteAtlasManager>()
                            .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")[
                            "small_bow_1"
                        ],
                        "Simple Bow",
                        "Just a bow----"
                    )
                );
            })
            .Named<Item>("simpleBowItem")
            .InstancePerLifetimeScope();

        builder
            .Register<Item>(x =>
            {
                return new WeaponItem(
                    new ItemInfo(
                        "simpleSpin",
                        x.Resolve<SpriteAtlasManager>()
                            .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")[
                            "chakram_1"
                        ],
                        "Simple Spin",
                        "Just a spin----"
                    )
                );
            })
            .Named<Item>("simpleSpinItem")
            .InstancePerLifetimeScope();

        builder
            .Register<Item>(x =>
            {
                return new WeaponItem(
                    new ItemInfo(
                        "simpleBoomerang",
                        x.Resolve<SpriteAtlasManager>()
                            .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")[
                            "boomerang_1"
                        ],
                        "Simple Boomerang",
                        "Just a boomerang----"
                    )
                );
            })
            .Named<Item>("simpleBoomerangItem")
            .InstancePerLifetimeScope();

        builder
            .Register<Item>(x =>
            {
                return new WeaponItem(
                    new ItemInfo(
                        "simpleKunai",
                        x.Resolve<SpriteAtlasManager>()
                            .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")[
                            "kunai_1"
                        ],
                        "Simple Kunai",
                        "Just a kunai----"
                    )
                );
            })
            .Named<Item>("simpleKunaiItem")
            .InstancePerLifetimeScope();

        builder
            .Register<Item>(x =>
            {
                return new WeaponItem(
                    new ItemInfo(
                        "simpleTrailBow",
                        x.Resolve<SpriteAtlasManager>()
                            .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")[
                            "small_bow_1"
                        ],
                        "Simple Trail Bow",
                        "Just a trail bow----"
                    )
                );
            })
            .Named<Item>("simpleTrailBowItem")
            .InstancePerLifetimeScope();

        builder
            .Register<Item>(x =>
            {
                return new WeaponItem(
                    new ItemInfo(
                        "simpleLaser",
                        x.Resolve<SpriteAtlasManager>()
                            .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")[
                            "stave_4"
                        ],
                        "Simple laser",
                        "Just a laser----"
                    )
                );
            })
            .Named<Item>("simpleLaserItem")
            .InstancePerLifetimeScope();

        builder
            .Register<Item>(x =>
            {
                return new WeaponItem(
                    new ItemInfo(
                        "simpleBook",
                        x.Resolve<SpriteAtlasManager>()
                            .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")["book_1"],
                        "Simple book",
                        "Just a book----"
                    )
                );
            })
            .Named<Item>("simpleBookItem")
            .InstancePerLifetimeScope();

        builder
            .Register<Item>(x =>
            {
                return new ShieldItem(
                    new ItemInfo(
                        "curseShield",
                        x.Resolve<SpriteAtlasManager>()
                            .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")[
                            "shield_1"
                        ],
                        "Curse shield",
                        "Instantly kills enemy who attacked you----"
                    )
                );
            })
            .Named<Item>("curseShieldItem")
            .InstancePerLifetimeScope();
        builder
            .Register<Item>(x =>
            {
                return new WeaponItem(
                    new ItemInfo(
                        "simpleCard",
                        x.Resolve<SpriteAtlasManager>()
                            .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")["card_1"],
                        "Simple card",
                        "Just a card----"
                    )
                );
            })
            .Named<Item>("simpleCardItem")
            .InstancePerLifetimeScope();
    }
}
