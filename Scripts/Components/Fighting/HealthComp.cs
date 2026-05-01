namespace Components.Fighting;

struct HealthComp
{
    public int currentHP,
        maxHP;

    public HealthComp(int maxHP)
    {
        this.currentHP = maxHP;
        this.maxHP = maxHP;
    }
}
