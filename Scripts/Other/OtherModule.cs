using Autofac;

namespace Other;

public class OtherModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ExpSys>().InstancePerLifetimeScope();
    }
}
