using Autofac;

namespace Other;

class OtherModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ExpSys>().InstancePerLifetimeScope();
    }
}
