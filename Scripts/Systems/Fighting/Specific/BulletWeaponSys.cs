using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Components.Other;
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
        ref CollComp coll,
        ref TimerComp timer
    )
    {
        if (!World.IsAlive(bullet.owner))
        {
            timer.time = 0.0f;
            return;
        }

        bullet.weapon.UpdateBullet(
            bullet.owner,
            bullet.extra,
            entity,
            ref trs,
            ref rigid,
            ref coll,
            ref timer
        );
    }
}
