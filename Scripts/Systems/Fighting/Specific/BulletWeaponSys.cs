using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Components.Other;
using Components.Physics;

namespace Systems.Fighting.Specific;

partial class BulletWeaponSys : BaseSystem<World, float>
{
    private readonly CommandBuffer _commBuffer;

    public BulletWeaponSys(World world, CommandBuffer commBuffer)
        : base(world)
    {
        _commBuffer = commBuffer;
    }

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
            if (!entity.Has<LocalTrsComp>())
                _commBuffer.Destroy(entity);
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
