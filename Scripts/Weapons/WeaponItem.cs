using Arch.Core;
using Arch.Core.Extensions;
using Autofac;
using Components.Basic;
using Components.Fighting;
using Other;

namespace Weapons;

public class WeaponItem : Item
{
    public WeaponItem(ItemInfo info)
        : base(info) { }

    public override void Pickup(ILifetimeScope scope, Entity entity)
    {
        WeaponElem elem = scope.ResolveNamed<WeaponElem>(info.prefabName);
        ref WeaponComp weapon = ref entity.Get<WeaponComp>();
        weapon.weapons.Add(elem);
        if (elem.entity != null)
            entity.Get<TrsComp>().descs!.Add(elem.entity.Value);
    }
}
