using Arch.Core;
using Arch.Core.Extensions;
using Autofac;
using Components.Basic;
using Components.Fighting;
using Other;

namespace Weapons;

public class ShieldItem : Item
{
    public ShieldItem(ItemInfo info)
        : base(info) { }

    public override void Pickup(ILifetimeScope scope, Entity entity)
    {
        ShieldElem elem = scope.ResolveNamed<ShieldElem>(info.prefabName);
        ref ShieldComp shield = ref entity.Get<ShieldComp>();
        shield.shields.Add(elem);
        if (elem.entity != null)
            entity.Get<TrsComp>().descs!.Add(elem.entity.Value);
    }
}
