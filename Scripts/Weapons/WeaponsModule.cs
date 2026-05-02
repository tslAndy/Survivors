using Autofac;
using Weapons.Specific;

namespace Weapons;

class WeaponsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<WeaponContext>().InstancePerLifetimeScope();
        builder.RegisterType<Bow>().InstancePerDependency();
    }
}
