using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Fighting;
using Other;

namespace Weapons;

public class ShieldItem : Item
{
    private readonly Func<string, ShieldElem> _resolver;

    public ShieldItem(Func<string, ShieldElem> resolver, string name, string description)
        : base(name, description) => _resolver = resolver;

    public override void Pickup(Entity entity)
    {
        ShieldElem elem = _resolver(name);
        ref ShieldComp shield = ref entity.Get<ShieldComp>();
        shield.shields.Add(elem);
        if (elem.entity != null)
            entity.Get<TrsComp>().descs!.Add(elem.entity.Value);
    }
}
