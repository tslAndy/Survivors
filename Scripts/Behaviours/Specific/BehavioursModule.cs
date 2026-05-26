using Autofac;

namespace Behaviours.Specific;

public class BehavioursModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<PlayerBehaviour>().InstancePerLifetimeScope();
        builder.RegisterType<GoblinBehaviour>().InstancePerLifetimeScope();
    }
}
