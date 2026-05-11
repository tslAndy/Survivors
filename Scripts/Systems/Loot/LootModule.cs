using Arch.Core;
using Autofac;
using Engine.Common;
using Engine.Sprites;
using Systems.Basic;
using Systems.Physics;

namespace Systems.Loot;

class LootModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register<DropSys>(x => new DropSys(
                x.Resolve<World>(),
                x.Resolve<SpriteAtlasManager>()
                    .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas"),
                x.Resolve<LayerMap>()["Loot"]
            ))
            .InstancePerLifetimeScope();

        builder
            .Register<LootCollectSys>(x => new LootCollectSys(
                x.Resolve<World>(),
                x.Resolve<SpatialSys>(),
                x.Resolve<ModRegistry>(),
                x.Resolve<LayerMap>()["Loot"]
            ))
            .InstancePerLifetimeScope();
    }
}
