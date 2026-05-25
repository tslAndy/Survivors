using Arch.Core;
using Autofac;
using Systems;
using Systems.Drawing;

namespace UI;

class UIModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register<MainUI>(x =>
            {
                UISys uiSys = x.Resolve<UISys>();

                NotifyUI achievesUI = new NotifyUI();
                uiSys.AddElem(achievesUI);

                StatsUI statsUI = new StatsUI(x.Resolve<WorldContext>());
                uiSys.AddElem(statsUI);

                ModsUI modsUI = new ModsUI(x.Resolve<WorldContext>());
                uiSys.AddElem(modsUI);

                DamageUI damageUI = new DamageUI(x.Resolve<World>());

                return new MainUI(achievesUI, statsUI, damageUI);
            })
            .InstancePerLifetimeScope();
    }
}
