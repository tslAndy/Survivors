using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Autofac;
using Systems.Animation;
using Systems.Basic;
using Systems.Behaviour;
using Systems.Drawing;
using Systems.Fighting;
using Systems.Fighting.Specific;
using Systems.Loot;
using Systems.Physics;

namespace Systems;

class SystemsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule(new BasicModule());
        builder.RegisterModule(new DrawingModule());
        builder.RegisterModule(new AnimSysModule());
        builder.RegisterModule(new FightingModule());
        builder.RegisterModule(new LootModule());
        builder.RegisterModule(new PhysicsModule());
        builder.RegisterModule(new BehaviourModule());

        builder
            .Register<Group<float>>(x =>
            {
                Group<float> systems = new Group<float>(
                    "Base Systems",
                    // basic
                    x.Resolve<LocalTrsSys>(),
                    // health
                    x.Resolve<HealthSys>(),
                    x.Resolve<DeathSys>(),
                    //physics
                    x.Resolve<SpatialSys>(),
                    x.Resolve<TileCollSys>(),
                    x.Resolve<RigidSys>(),
                    // character systems
                    x.Resolve<BehaviourSys>(),
                    // weapon systems
                    x.Resolve<BulletWeaponSys>(),
                    // fighting
                    x.Resolve<WeaponSys>(),
                    x.Resolve<ShieldSys>(),
                    // damage
                    x.Resolve<StatusEffectSys>(),
                    x.Resolve<DamageSys>(),
                    // loot
                    x.Resolve<LootDropSys>(),
                    x.Resolve<LootCollectSys>(),
                    // audio and drawing
                    x.Resolve<AnimSys>(),
                    x.Resolve<TilemapDrawSys>(),
                    x.Resolve<SpriteDrawSys>(),
                    x.Resolve<LineDrawSys>(),
                    x.Resolve<TextDrawSys>(),
                    x.Resolve<SoundSys>()
                );
                systems.Initialize();
                return systems;
            })
            .InstancePerLifetimeScope();

        builder.Register<World>(_ => World.Create()).InstancePerLifetimeScope();
        builder.RegisterType<CommandBuffer>().InstancePerLifetimeScope();
        builder.RegisterType<WorldContext>().InstancePerLifetimeScope();
    }
}
