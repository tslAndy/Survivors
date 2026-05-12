using Autofac;

namespace Systems.Fighting.Specific;

class SpecificWeaponModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BulletWeaponSys>();
    }
}
