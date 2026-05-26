using Arch.Bus;
using Arch.Core.Extensions;
using Components.Basic;
using Events;

namespace Achieves.Specific;

public partial class KillAchieve : CountAchieve
{
    public KillAchieve(int target, string name, string description)
        : base(target, name, description) => Hook();

    [Event]
    public void OnDeath(ref DeathEvent @event)
    {
        if (@event.entity.Has<EnemyTag>())
            IncrementCount();
    }
}
