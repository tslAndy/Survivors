using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Other;
using Components.Physics;
using Systems;

namespace Weapons.Specific;

class Boomerang : Bow
{
    public Boomerang(
        BulletConfig bulletConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context
    )
        : base(bulletConfig, config, callbacks, context) { }

    public override void UpdateBullet(
        Entity owner,
        Entity? extra,
        Entity bullet,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref CollComp coll,
        ref TimerComp timer
    )
    {
        base.UpdateBullet(owner, extra, bullet, ref trs, ref rigid, ref coll, ref timer);

        ref TrsComp ownerTrs = ref owner.Get<TrsComp>();
        Vector2 delta = ownerTrs.position - trs.position;
        if (delta.LengthSquared() < config.detectRadius * config.detectRadius)
            return;

        // if bullet moves towards player
        if (Vector2.Dot(delta, rigid.velocity) > 0.0f)
            return;

        delta = Vector2.Normalize(delta);
        float speed = rigid.velocity.Length();
        rigid.velocity = delta * speed;
    }
}
