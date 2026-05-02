using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Autofac;
using Systems.Animation;
using Systems.Characters;
using Systems.Drawing;
using Systems.Fighting;
using Systems.Loot;
using Systems.Physics;

namespace Systems;

class SystemsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule(new DrawingModule());
        builder.RegisterModule(new AnimSysModule());
        builder.RegisterModule(new FightingModule());
        builder.RegisterModule(new LootModule());
        builder.RegisterModule(new PhysicsModule());
        builder.RegisterModule(new CharactersModule());

        builder
            .Register<Group<float>>(x =>
            {
                Group<float> systems = new Group<float>(
                    "Base Systems",
                    //physics
                    x.Resolve<SpatialSys>(),
                    x.Resolve<TileCollSys>(),
                    x.Resolve<RigidSys>(),
                    // player systems
                    x.Resolve<PlayerSys>(),
                    x.Resolve<EnemyMoveSys>(),
                    // fighting
                    x.Resolve<HealthSys>(),
                    x.Resolve<DeathSys>(),
                    x.Resolve<WeaponSys>(),
                    x.Resolve<BulletSys>(),
                    x.Resolve<ShieldSys>(),
                    x.Resolve<StatusEffectSys>(),
                    x.Resolve<DamageSys>(),
                    // loot
                    x.Resolve<DropSys>(),
                    x.Resolve<LootCollectSys>(),
                    // audio and drawing
                    x.Resolve<AnimSys>(),
                    x.Resolve<TilemapDrawSys>(),
                    x.Resolve<SpriteDrawSys>(),
                    x.Resolve<TextDrawSys>(),
                    x.Resolve<SoundSys>()
                );
                systems.Initialize();
                return systems;
            })
            .InstancePerLifetimeScope();

        builder.Register<World>(_ => World.Create()).InstancePerLifetimeScope();
        builder.RegisterType<CommandBuffer>().InstancePerLifetimeScope();
    }
}
