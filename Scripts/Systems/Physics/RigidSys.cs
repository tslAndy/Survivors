using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Physics;

namespace Systems.Physics;

partial class RigidSys : BaseSystem<World, float>
{
    public RigidSys(World world)
        : base(world) { }

    [Query]
    [None(typeof(DeathComp))]
    private void Move([Data] in float dt, ref TransformComp trs, in RigidComp rigid)
    {
        trs.position += rigid.velocity * dt;
    }
}

// private readonly TileCollSys _tileCollSys;
// private readonly CachedList<TileColl> _tileColls;
//
// [Query]
// private void SolveColl(ref TransformComp trs, ref RigidComp rigid, in CollComp coll)
// {
//     _tileCollSys.GetOverlap(in coll, in trs, in rigid.layer, in _tileColls);
//
//     for (int i = 0; i < _tileColls.Count; i++)
//     {
//         ref TileColl tileColl = ref _tileColls[i];
//
//         if (Vector2.Dot(rigid.velocity, tileColl.normal) > 0.001f)
//             continue;
//
//         rigid.velocity = Vector2.Reflect(rigid.velocity, tileColl.normal);
//     }
//
//     _tileColls.Reset();
// }
