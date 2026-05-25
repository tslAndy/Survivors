using Systems.Drawing;

namespace UI;

class MainUI
{
    public readonly AchievesUI achievesUI;

    public MainUI(UISys uiSys)
    {
        achievesUI = new AchievesUI();

        uiSys.AddElem(achievesUI);
    }
}
