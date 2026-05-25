using Arch.Bus;
using Events;

namespace Other;

partial class ExpSys
{
    public int currentExp { get; private set; } = 0;
    public int totalExp { get; private set; } = START_EXP;

    public int level { get; private set; } = 1;

    private const int START_EXP = 1000;
    private const float RATE = 1.5f; // next level requires 1.5 exp

    public ExpSys() => Hook();

    [Event]
    public void OnExpCollect(ref ExpCollectEvent @event)
    {
        currentExp += @event.amount;
        if (currentExp < totalExp)
            return;

        currentExp -= totalExp;
        totalExp = (int)MathF.Floor(RATE * totalExp);
        level++;
    }
}
