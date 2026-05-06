using Autofac;

namespace Systems.Basic;

class BasicModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TrsOwningSys>().InstancePerLifetimeScope();
    }
}
