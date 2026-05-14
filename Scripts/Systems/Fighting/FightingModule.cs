using Autofac;

namespace Systems.Fighting;

class FightingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BulletWeaponSys>().InstancePerLifetimeScope();
        builder.RegisterType<WeaponSys>().InstancePerLifetimeScope();
        builder.RegisterType<ShieldSys>().InstancePerLifetimeScope();
    }
}
