using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Components.Physics;

namespace Systems.Fighting.Specific;

partial class BulletWeaponSys : BaseSystem<World, float>
{
    public BulletWeaponSys(World world)
        : base(world) { }

    [Query]
    private void UpdateBullet(
        Entity entity,
        in BulletComp bullet,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref CollComp coll
    )
    {
        bullet.weapon.UpdateBullet(bullet.owner, entity, ref trs, ref rigid, ref coll);
    }
}
