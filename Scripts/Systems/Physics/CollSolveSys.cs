using System.Numerics;
using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Physics;
using Engine.Common;
using Systems.Physics;
using Utils;

partial class CollSolveSys : BaseSystem<World, float>
{
    private readonly LayerMap _layerMap;
    private readonly SpatialSys _spatialSys;
    private readonly TileCollSys _tileSys;
    private readonly CommandBuffer _commBuffer;
    private readonly int _wallsLayer;

    private const float CORR_RATE = 0.4f,
        CORR_SLOP = 0.001f;

    public CollSolveSys(
        World world,
        LayerMap layerMap,
        SpatialSys spatialSys,
        TileCollSys tileSys,
        CommandBuffer commBuffer
    )
        : base(world)
    {
        _layerMap = layerMap;
        _spatialSys = spatialSys;
        _tileSys = tileSys;
        _commBuffer = commBuffer;
        _wallsLayer = _layerMap["Walls"];
    }

    [Query]
    [None(typeof(DeathComp))]
    private void UpdateCollision(
        Entity entity,
        in TrsComp trs,
        in RigidComp rigid,
        in CollComp coll
    )
    {
        if (!_layerMap.GetCollSolve(rigid.layer))
            return;

        // solve self collisions
        using CachedList<Entity> collOverlap = CachedList<Entity>.Create();
        _spatialSys.GetOverlap(entity, trs.position, coll.radius, rigid.layer, collOverlap);

        Vector2 corr = Vector2.Zero;
        for (int i = 0; i < collOverlap.Count; i++)
        {
            Components<TrsComp, RigidComp, CollComp> comps = collOverlap[i]
                .Get<TrsComp, RigidComp, CollComp>();

            ref TrsComp otherTrs = ref comps.t0;
            ref RigidComp otherRigid = ref comps.t1;
            ref CollComp otherColl = ref comps.t2;

            if (Vector2.Dot(rigid.velocity, otherRigid.velocity) < 0.0f)
                continue;

            Vector2 delta = trs.position - otherTrs.position;
            float targDist = coll.radius + otherColl.radius;

            if (delta.LengthSquared() > (targDist - CORR_SLOP) * (targDist - CORR_SLOP))
                continue;

            float deltaLen = delta.Length();
            float overlap = targDist - deltaLen;

            delta /= deltaLen;
            corr += CORR_RATE * overlap * delta;
        }

        using CachedList<TileColl> tileOverlap = CachedList<TileColl>.Create();
        _tileSys.GetOverlap(trs.position, coll.radius, _wallsLayer, tileOverlap);

        for (int i = 0; i < tileOverlap.Count; i++)
        {
            ref TileColl tileColl = ref tileOverlap[i];
            corr += CORR_RATE * tileColl.depth * tileColl.normal;
        }

        if (collOverlap.Count != 0 || tileOverlap.Count != 0)
            _commBuffer.Set<TrsComp>(entity, trs with { position = trs.position + corr });
    }
}
