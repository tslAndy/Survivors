using Achieves.Specific;
using Arch.Core;
using Autofac;
using Components.Basic;

namespace Achieves;

class AchievesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register<AchieveSys>(x => new AchieveSys(
                // kills
                new KillAchieve(10, "10 kills", "Kill ten enemies"),
                new KillAchieve(50, "50 kills", "Kill fifty enemies"),
                new KillAchieve(100, "100 kills", "Kill hundred enemies"),
                // exp
                new ExpCollectAchieve(1000, "1000 exp", ""),
                new ExpCollectAchieve(10000, "10K exp", ""),
                new ExpCollectAchieve(15000, "15K exp", ""),
                // damage
                new DamageAchieve(
                    Component<EnemyTag>.ComponentType,
                    10_000,
                    "10K damage",
                    "Give enemies 10_000 damage"
                ),
                new DamageAchieve(
                    Component<EnemyTag>.ComponentType,
                    20_000,
                    "20K damage",
                    "Give enemies 20_000 damage"
                ),
                new DamageAchieve(
                    Component<PlayerTag>.ComponentType,
                    1_000,
                    "Cannon fodder",
                    "Take 1_000 damage"
                ),
                new DamageAchieve(
                    Component<PlayerTag>.ComponentType,
                    5_000,
                    "Cannon fodder plus",
                    "Take 5_000 damage"
                )
            ))
            .InstancePerLifetimeScope();
    }
}
