using Autofac;

namespace Systems.Animation;

class AnimSysModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AnimSys>().InstancePerLifetimeScope();
        builder.RegisterType<SoundSys>().InstancePerLifetimeScope();
    }
}
