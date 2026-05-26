using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Fighting;
using Other;

namespace Weapons;

public class WeaponItem : Item
{
    private readonly Func<string, WeaponElem> _resolver;

    public WeaponItem(Func<string, WeaponElem> resolver, string name, string description)
        : base(name, description) => _resolver = resolver;

    public override void Pickup(Entity entity)
    {
        WeaponElem elem = _resolver(name);
        ref WeaponComp weapon = ref entity.Get<WeaponComp>();
        weapon.weapons.Add(elem);
        if (elem.entity != null)
            entity.Get<TrsComp>().descs!.Add(elem.entity.Value);
    }
}
