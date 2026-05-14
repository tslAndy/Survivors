using Autofac;

namespace Systems.Basic;

class BasicModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<LocalTrsSys>().InstancePerLifetimeScope();
        builder.RegisterType<TimerSys>().InstancePerLifetimeScope();
    }
}
