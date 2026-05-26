using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Physics;

namespace Systems.Physics;

public partial class RigidSys : BaseSystem<World, float>
{
    public RigidSys(World world)
        : base(world) { }

    [Query]
    [None(typeof(DeathComp), typeof(LocalTrsComp))]
    private void MoveGlobal([Data] in float dt, ref TrsComp trs, in RigidComp rigid)
    {
        trs.position += rigid.velocity * dt;
        trs.rotation += rigid.rotVelocity * dt;
    }

    [Query]
    [None(typeof(DeathComp))]
    private void MoveLocal([Data] in float dt, ref LocalTrsComp localTrs, in RigidComp rigid)
    {
        localTrs.position += rigid.velocity * dt;
        localTrs.rotation += rigid.rotVelocity * dt;
    }
}
