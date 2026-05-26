using Arch.Bus;
using Arch.Core;
using Arch.System;
using Events;

namespace Systems.Loot;

public partial class ExpSys : BaseSystem<World, float>
{
    public int currentExp { get; private set; } = 0;
    public int totalExp { get; private set; } = START_EXP;

    public int level { get; private set; } = 1;

    private const int START_EXP = 1000;
    private const float RATE = 1.5f; // next level requires 1.5 exp

    public ExpSys(World world)
        : base(world) => Hook();

    public override void Update(in float dt)
    {
        if (currentExp < totalExp)
            return;

        currentExp -= totalExp;
        totalExp = (int)MathF.Floor(RATE * totalExp);
        level++;

        LevelupEvent levelupEvent = new LevelupEvent { level = level };
        EventBus.Send(ref levelupEvent);
    }

    [Event]
    public void OnExpCollect(ref ExpCollectEvent @event)
    {
        currentExp += @event.amount;
    }
}
