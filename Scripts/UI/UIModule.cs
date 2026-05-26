using Arch.Core;
using Autofac;
using Systems;
using Systems.Basic;
using Systems.Drawing;
using Systems.Loot;

namespace UI;

public class UIModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register<MainUI>(x =>
            {
                MainUI mainUI = new MainUI(
                    new NotifyUI(),
                    new StatsUI(
                        x.Resolve<WorldContext>(),
                        x.Resolve<ExpSys>(),
                        x.Resolve<LevelSys>()
                    ),
                    new ModsUI(x.Resolve<WorldContext>()),
                    new LevelupUI(x.Resolve<LevelSys>(), x.Resolve<WorldContext>())
                    {
                        isActive = false,
                    },
                    new DamageUI(x.Resolve<World>())
                );

                UISys uiSys = x.Resolve<UISys>();
                uiSys.AddElem(mainUI.statsUI);
                uiSys.AddElem(mainUI.modsUI);
                uiSys.AddElem(mainUI.notifyUI);
                uiSys.AddElem(mainUI.levelupUI);

                return mainUI;
            })
            .InstancePerLifetimeScope();
    }
}
