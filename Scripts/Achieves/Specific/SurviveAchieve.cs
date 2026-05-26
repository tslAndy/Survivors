using Arch.Bus;
using Events;

namespace Achieves.Specific;

public partial class SurviveAchieve : CountAchieve
{
    private readonly string _levelName;

    public SurviveAchieve(string levelName, int target, string name, string description)
        : base(target, name, description)
    {
        _levelName = levelName;
        Hook();
    }

    [Event]
    public void OnSecPassed(ref SecPassEvent @event)
    {
        if (_levelName == @event.levelName)
            IncrementCount();
    }
}
