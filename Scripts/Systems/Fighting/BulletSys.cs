using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Components.Physics;

namespace Systems.Fighting;

partial class BulletSys : BaseSystem<World, float>
{
    public BulletSys(World world)
        : base(world) { }

    [Query]
    private void Update(
        Entity entity,
        ref BulletComp bulletComp,
        ref CollComp coll,
        ref TransformComp trs
    )
    {
        bulletComp.weapon.UpdateBullet(
            bulletComp.owner,
            entity,
            trs.position,
            trs.scale * coll.radius
        );
    }
}
