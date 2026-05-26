using Arch.Core;
using Autofac;
using Engine.Common;
using Engine.Sprites;

namespace Systems.Loot;

public class LootModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register<LootDropSys>(x => new LootDropSys(
                x.Resolve<World>(),
                x.Resolve<SpriteAtlasManager>()
                    .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas"),
                x.Resolve<LayerMap>()
            ))
            .InstancePerLifetimeScope();

        builder.RegisterType<LootCollectSys>().InstancePerLifetimeScope();
        builder.RegisterType<ExpSys>().InstancePerLifetimeScope();
    }
}
