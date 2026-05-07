using Arch.Core;
using Autofac;
using Components.Basic;
using Components.Fighting;
using Engine.Animations;
using Engine.Common;
using Systems;
using Weapons.Specific;

namespace Weapons;

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
                    attackTime = 0.75f,
                    detectRadius = 4.0f,
                    targetLayer = x.Resolve<LayerMap>()["EnemyEnts"],
                };

                WeaponCallbacks callbacks = new WeaponCallbacks { };

                AnimAtlas swingAtlas = x.Resolve<AnimAtlasManager>()
                    .Get("./Resources/AnimAtlases/Items/BattleEffects.animAtlas");

                Entity swing = x.Resolve<World>()
                    .Create<TransformComp, SpriteComp, AnimComp>(
                        new TransformComp { scale = 1.0f },
                        new SpriteComp { drawOrder = 2 },
                        new AnimComp
                        {
                            anim = swingAtlas["Swing_A"],
                            atlas = swingAtlas,
                            timeScale = 1.0f,
                        }
                    );

                Weapon weapon = new MeleeWeapon(config, callbacks, x.Resolve<WorldContext>());
                return new WeaponElem(weapon, swing);
            })
            .Named<WeaponElem>("simpleSword")
            .InstancePerLifetimeScope();
    }
}
