using Arch.Bus;
using Arch.Core;
using Arch.Core.Extensions;
using Events;

namespace Achieves.Specific;

partial class DamageAchieve : CountAchieve
{
    private readonly ComponentType _tag;

    public DamageAchieve(ComponentType tag, int target, string name, string description)
        : base(target, name, description)
    {
        _tag = tag;
        Hook();
    }

    [Event]
    public void OnDamage(ref DamageEvent @event)
    {
        if (@event.target.Has(_tag) && @event.damage > 0)
            IncrementCount(@event.damage);
    }
}
