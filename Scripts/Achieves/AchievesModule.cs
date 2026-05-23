using Achieves.Specific;
using Autofac;

namespace Achieves;

class AchievesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register<AchiveContainer>(x => new AchiveContainer(
                new KillAchieve(10, "Tens", "Kill ten enemies"),
                new KillAchieve(50, "Fifty", "Kill fifty enemies"),
                new KillAchieve(100, "Hundred", "Kill hundred enemies")
            ))
            .InstancePerLifetimeScope();
    }
}
