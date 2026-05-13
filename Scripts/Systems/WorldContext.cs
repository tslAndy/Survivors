using System.Numerics;
using Arch.Buffer;
using Arch.Core;
using Engine.Common;
using Systems.Animation;
using Systems.Physics;

namespace Systems;

class WorldContext
{
    public readonly World world;
    public readonly SpatialSys spatialSys;
    public readonly TileCollSys tileSys;
    public readonly SoundSys soundSys;
    public readonly CommandBuffer commandBuffer;
    public readonly LayerMap layerMap;

    public Entity player;
    public Vector2 playerPos;

    public WorldContext(
        World world,
        SpatialSys spatial,
        TileCollSys tileCollSys,
        SoundSys soundSys,
        CommandBuffer commandBuffer,
        LayerMap layerMap
    )
    {
        this.world = world;
        this.spatialSys = spatial;
        this.tileSys = tileCollSys;
        this.soundSys = soundSys;
        this.commandBuffer = commandBuffer;
        this.layerMap = layerMap;
    }
}
