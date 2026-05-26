namespace UI;

public class MainUI
{
    public readonly NotifyUI notifyUI;
    public readonly StatsUI statsUI;
    public readonly DamageUI damageUI;
    public readonly ModsUI modsUI;
    public readonly LevelupUI levelupUI;

    public MainUI(
        NotifyUI notifyUI,
        StatsUI statsUI,
        ModsUI modsUI,
        LevelupUI levelupUI,
        DamageUI damageUI
    )
    {
        this.notifyUI = notifyUI;
        this.statsUI = statsUI;
        this.modsUI = modsUI;
        this.levelupUI = levelupUI;
        this.damageUI = damageUI;
    }
}
