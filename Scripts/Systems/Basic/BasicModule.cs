using Autofac;

namespace Systems.Basic;

class BasicModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<LocalTrsSys>().InstancePerLifetimeScope();
        builder
            .Register<ModRegistry>(x => new ModRegistry(
                "moveFactor",
                "animTimeFactor",
                "attackSpeedFactor",
                "detectRadiusFactor",
                "bulletSpeedFactor",
                "damageGiveFactor",
                "damageTakeFactor",
                "lootRadiusFactor",
                "lootIncomeFactor"
            ))
            .InstancePerLifetimeScope();
    }
}
