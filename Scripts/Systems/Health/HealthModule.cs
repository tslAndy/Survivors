using Autofac;

namespace Systems.Health;

public class HealthModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<StatusEffectSys>().InstancePerLifetimeScope();
        builder.RegisterType<StatusEffectHandler>().InstancePerLifetimeScope();

        builder.RegisterType<DamageSys>().InstancePerLifetimeScope();
        builder.RegisterType<HealthSys>().InstancePerLifetimeScope();

        builder.RegisterType<DeathSys>().InstancePerLifetimeScope();
    }
}
