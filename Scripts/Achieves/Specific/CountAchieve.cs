namespace Achieves.Specific;

public abstract class CountAchieve : Achieve
{
    private int _count,
        _target;

    protected CountAchieve(int target, string name, string description)
        : base(name, description)
    {
        _target = target;
    }

    protected void IncrementCount(int amount = 1)
    {
        _count += amount;
        if (_count >= _target)
            TryUnlock();
    }
}
