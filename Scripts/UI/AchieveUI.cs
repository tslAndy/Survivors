using Achieves;
using Arch.Bus;

namespace UI;

partial class AchieveUI
{
    public AchieveUI() => Hook();

    [Event]
    public void OnAchieveUnlocked(Achieve achieve)
    {
        Console.WriteLine($"{achieve.name}: {achieve.description}");
    }
}
