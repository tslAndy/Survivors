using Autofac;

namespace Systems.Basic;

public class BasicModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<LocalTrsSys>().InstancePerLifetimeScope();
        builder.RegisterType<TimerSys>().InstancePerLifetimeScope();
        builder.RegisterType<LevelSys>().InstancePerLifetimeScope();
    }
}
