using Autofac;

namespace Systems.Characters;

class CharactersModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<PlayerSys>().InstancePerLifetimeScope();
    }
}
