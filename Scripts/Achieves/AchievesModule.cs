using Achieves.Specific;
using Autofac;

namespace Achieves;

class AchievesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register<AchiveContainer>(x => new AchiveContainer(
                new KillAchieve(10, "10 kills", "Kill ten enemies"),
                new KillAchieve(50, "50 kills", "Kill fifty enemies"),
                new KillAchieve(100, "100 kills", "Kill hundred enemies"),
                new ExpCollectAchieve(1000, "1000 exp", ""),
                new ExpCollectAchieve(10000, "10K exp", ""),
                new ExpCollectAchieve(15000, "15K exp", "")
            ))
            .InstancePerLifetimeScope();
    }
}
