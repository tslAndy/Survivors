using Arch.Bus;
using Arch.Core.Extensions;
using Components.Basic;
using Events;

namespace Achieves.Specific;

partial class KillAchieve : Achieve
{
    private readonly int _target;
    private int _count;

    public KillAchieve(int target, string name, string description)
        : base(name, description)
    {
        _target = target;
        Hook();
    }

    [Event]
    public void OnDeathEvent(ref DeathEvent @event)
    {
        if (@event.entity.Has<EnemyTag>())
            _count++;

        if (_count >= _target)
            TryUnlock();
    }
}
