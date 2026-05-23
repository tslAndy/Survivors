namespace Achieves;

class AchiveContainer
{
    private readonly Achieve[] _achievements;

    public AchiveContainer(params Achieve[] achievements) => _achievements = achievements;
}
