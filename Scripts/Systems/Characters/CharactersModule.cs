using Arch.Core;
using Autofac;
using Engine.Common;

namespace Systems.Characters;

class CharactersModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register<EnemyMoveSys>(x => new EnemyMoveSys(
                x.Resolve<World>(),
                x.Resolve<LayerMap>()["PlayerEnts"]
            ))
            .InstancePerLifetimeScope();

        builder.RegisterType<PlayerSys>().InstancePerLifetimeScope();
    }
}
