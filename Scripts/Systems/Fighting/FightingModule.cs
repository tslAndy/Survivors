using Autofac;

namespace Systems.Fighting;

class FightingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<WeaponSys>().InstancePerLifetimeScope();
        builder.RegisterType<ShieldSys>().InstancePerLifetimeScope();
        builder.RegisterType<StatusEffectSys>().InstancePerLifetimeScope();
        builder.RegisterType<DamageSys>().InstancePerLifetimeScope();
        builder.RegisterType<HealthSys>().InstancePerLifetimeScope();
        builder.RegisterType<DeathSys>().InstancePerLifetimeScope();
    }
}
