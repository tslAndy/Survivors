using Autofac;

namespace UI;

class UIModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AchieveUI>().InstancePerLifetimeScope();
    }
}
