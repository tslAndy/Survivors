using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Autofac;
using Systems.Animation;
using Systems.Basic;
using Systems.Behaviour;
using Systems.Drawing;
using Systems.Fighting;
using Systems.Health;
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
        builder.RegisterModule(new HealthModule());
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
                    x.Resolve<RigidSys>(),
                    x.Resolve<LocalTrsSys>(),
                    x.Resolve<SpatialSys>(),
                    x.Resolve<TileCollSys>(),
                    // timer
                    x.Resolve<TimerSys>(),
                    // character systems
                    x.Resolve<BehaviourSys>(),
                    // fighting
                    x.Resolve<WeaponSys>(),
                    x.Resolve<BulletWeaponSys>(),
                    x.Resolve<ShieldSys>(),
                    // damage
                    x.Resolve<StatusEffectSys>(),
                    x.Resolve<DamageSys>(),
                    x.Resolve<HealthSys>(),
                    // loot
                    x.Resolve<LootDropSys>(),
                    x.Resolve<LootCollectSys>(),
                    // correction
                    x.Resolve<CollSolveSys>(),
                    // audio and drawing
                    x.Resolve<AnimSys>(),
                    x.Resolve<TilemapDrawSys>(),
                    x.Resolve<SpriteDrawSys>(),
                    x.Resolve<LineDrawSys>(),
                    x.Resolve<TextDrawSys>(),
                    x.Resolve<SoundSys>(),
                    // death
                    x.Resolve<DeathSys>()
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
