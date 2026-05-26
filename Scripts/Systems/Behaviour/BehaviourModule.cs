using Autofac;

namespace Systems.Behaviour;

public class BehaviourModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BehaviourSys>().InstancePerLifetimeScope();
    }
}
