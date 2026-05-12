using Autofac;

namespace Systems.Behaviour;

class BehaviourModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BehaviourSys>().InstancePerLifetimeScope();
    }
}
