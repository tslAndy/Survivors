namespace UI;

class MainUI
{
    public readonly NotifyUI notifyUI;
    public readonly StatsUI statsUI;
    public readonly DamageUI damageUI;

    public MainUI(NotifyUI notifyUI, StatsUI statsUI, DamageUI damageUI)
    {
        this.notifyUI = notifyUI;
        this.statsUI = statsUI;
        this.damageUI = damageUI;
    }
}
