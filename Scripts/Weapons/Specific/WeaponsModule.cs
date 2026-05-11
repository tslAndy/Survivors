using Arch.Core;
using Autofac;
using Components.Basic;
using Components.Fighting;
using Engine.Animations;
using Engine.Common;
using Engine.Sprites;
using Raylib_cs;
using Systems;
using Systems.Basic;

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
                    baseDamage = 20,
                    critDamage = 50,
                    critChance = 30,
                    attackTime = 0.5f,
                    detectRadius = 8.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                };

                BulletConfig bulletConfig = new BulletConfig
                {
                    sprite = x.Resolve<SpriteAtlasManager>()
                        .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")["arrow_1"],
                    velocity = 8.0f,
                    radius = 0.25f,
                    bulletLayer = x.Resolve<LayerMap>()["PlayerBullets"],
                    drawOrder = 2,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                IWeapon weapon = new Bow(
                    bulletConfig,
                    config,
                    callbacks,
                    x.Resolve<WorldContext>(),
                    x.Resolve<ModRegistry>()
                );
                return new WeaponElem(weapon, null);
            })
            .Named<WeaponElem>("simpleBow")
            .InstancePerDependency();

        builder
            .Register<WeaponElem>(x =>
            {
                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 2,
                    critDamage = 5,
                    critChance = 30,
                    attackTime = 0.1f,
                    detectRadius = 4.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                RayConfig rayConfig = new RayConfig
                {
                    rays = 10,
                    length = 10.0f,
                    rotationSpeed = Single.DegreesToRadians(180.0f),
                    thick = 0.1f,
                    color = Color.Red,
                };

                IWeapon weapon = new Laser(
                    rayConfig,
                    config,
                    callbacks,
                    x.Resolve<WorldContext>(),
                    x.Resolve<ModRegistry>()
                );
                return new WeaponElem(weapon, null);
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
                    attackTime = 0.75f,
                    detectRadius = 4.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                AnimAtlas swingAtlas = x.Resolve<AnimAtlasManager>()
                    .Get("./Resources/AnimAtlases/Items/BattleEffects.animAtlas");

                Entity swing = x.Resolve<World>()
                    .Create<TrsComp, LocalTrsComp, SpriteComp, AnimComp>(
                        new TrsComp { scale = 1.0f },
                        new LocalTrsComp { scale = 1.0f },
                        new SpriteComp { drawOrder = 2 },
                        new AnimComp { anim = swingAtlas["Swing_1"], atlas = swingAtlas }
                    );

                IWeapon weapon = new MeleeWeapon(
                    config,
                    callbacks,
                    x.Resolve<WorldContext>(),
                    x.Resolve<ModRegistry>()
                );
                return new WeaponElem(weapon, null);
            })
            .Named<WeaponElem>("simpleSword")
            .InstancePerDependency();
    }
}
