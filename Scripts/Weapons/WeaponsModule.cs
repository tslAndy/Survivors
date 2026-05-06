using Autofac;
using Weapons.Specific;

namespace Weapons;

class WeaponsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // WARNING: каждое оружие или щит инстанциируется как уникальное
        // для врагов сохраняем одно инстанциированное оружие и добавляем ко всем
        // таким образом можно использовать оружия одного и того же типа но с разными параметрами
        // враги не имеют компонент оружия, у них отдельный контроллер

        builder.RegisterType<WeaponContext>().InstancePerLifetimeScope();
        builder.RegisterType<MeleeWeapon>().InstancePerDependency();
    }
}
