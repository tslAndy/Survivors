using Arch.Core;
using Arch.Core.Extensions;
using Autofac;
using Components.Basic;
using Components.Fighting;
using Engine.Animations;
using Engine.Common;
using Engine.Sounds;
using Engine.Sprites;
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
                    lifetime = 4.0f,
                    bulletLayer = x.Resolve<LayerMap>()["PlayerBullets"],
                    drawOrder = 2,
                    perforate = true,
                    bounce = true,
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                IWeapon weapon = new BulletWeapon(
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
                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 20,
                    critDamage = 50,
                    critChance = 30,
                    attackTime = 0.75f,
                    detectRadius = 2.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
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
                SpinConfig spinConfig = new SpinConfig
                {
                    sprite = x.Resolve<SpriteAtlasManager>()
                        .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas")["chakram_1"],

                    rotSpeed = 90.0f,
                    circleRadius = 2.5f,
                    bulletRadius = 0.5f,
                    bulletsCount = 10,
                    drawOrder = 2,
                };

                WeaponConfig config = new WeaponConfig
                {
                    baseDamage = 10,
                    critDamage = 20,
                    critChance = 30,
                    attackTime = 0.75f,
                    detectRadius = 2.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                };

                WeaponCallbacks callbacks = new WeaponCallbacks
                {
                    onBaseDamage = (attacker, target, ref val) =>
                    {
                        attacker
                            .Get<StatusEffectComp>()
                            .newEffects.Add(
                                new StatusEffect(StatusEffectType.AttackSpeedIncrease, 10.0f, 3.0f)
                            );
                    },
                };

                IWeapon weapon = new SpinWeapon(
                    spinConfig,
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
    }
}
