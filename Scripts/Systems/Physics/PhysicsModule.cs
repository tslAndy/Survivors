using Autofac;

namespace Systems.Physics;

public class PhysicsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SpatialSys>().InstancePerLifetimeScope();
        builder.RegisterType<TileCollSys>().InstancePerLifetimeScope();
        builder.RegisterType<RigidSys>().InstancePerLifetimeScope();
        builder.RegisterType<CollSolveSys>().InstancePerLifetimeScope();
    }
}
