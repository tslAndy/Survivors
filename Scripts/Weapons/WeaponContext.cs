using Arch.Buffer;
using Arch.Core;
using Engine.Common;
using Systems.Physics;

namespace Weapons;

class WeaponContext
{
    public readonly World world;
    public readonly SpatialSys spatial;
    public readonly TileCollSys tileCollSys;
    public readonly CommandBuffer commandBuffer;
    public readonly LayerMap layerMap;

    public WeaponContext(
        World world,
        SpatialSys spatial,
        TileCollSys tileCollSys,
        CommandBuffer commandBuffer,
        LayerMap layerMap
    )
    {
        this.world = world;
        this.spatial = spatial;
        this.tileCollSys = tileCollSys;
        this.commandBuffer = commandBuffer;
        this.layerMap = layerMap;
    }
}
