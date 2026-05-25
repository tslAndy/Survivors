using Autofac;
using Systems.Drawing;

namespace UI;

class UIModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register<MainUI>(x => new MainUI(x.Resolve<UISys>())).InstancePerLifetimeScope();
    }
}
