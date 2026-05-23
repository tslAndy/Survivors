using Arch.Bus;

namespace Achieves;

public abstract class Achieve
{
    public readonly string name,
        description;

    private bool _unlocked;

    protected Achieve(string name, string description)
    {
        this.name = name;
        this.description = description;
    }

    protected bool TryUnlock()
    {
        if (_unlocked)
            return false;

        _unlocked = true;
        EventBus.Send(this);
        return true;
    }
}
