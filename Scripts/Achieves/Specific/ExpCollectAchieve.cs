using Arch.Bus;
using Events;

namespace Achieves.Specific;

public partial class ExpCollectAchieve : CountAchieve
{
    public ExpCollectAchieve(int target, string name, string description)
        : base(target, name, description) => Hook();

    [Event]
    public void OnExpCollect(ref ExpCollectEvent @event) => IncrementCount(@event.amount);
}
