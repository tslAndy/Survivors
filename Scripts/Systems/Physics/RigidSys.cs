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
    private void Move([Data] in float dt, ref TrsComp trs, in RigidComp rigid)
    {
        trs.position += rigid.velocity * dt;
    }
}
