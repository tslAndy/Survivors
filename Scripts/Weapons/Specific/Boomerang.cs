using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Physics;
using Raylib_cs;
using Systems;

namespace Weapons.Specific;

struct BoomerangConfig
{
    public float maxDist;
    public float rotSpeed;
}

class Boomerang : Bow
{
    protected readonly BoomerangConfig boomrConfig;

    public Boomerang(
        BoomerangConfig boomerangConfig,
        BulletConfig bulletConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context
    )
        : base(bulletConfig, config, callbacks, context)
    {
        this.boomrConfig = boomerangConfig;
    }

    public override void UpdateBullet(
        Entity owner,
        Entity? extra,
        Entity bullet,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref CollComp coll
    )
    {
        base.UpdateBullet(owner, extra, bullet, ref trs, ref rigid, ref coll);

        trs.rotation += Raylib.GetFrameTime() * boomrConfig.rotSpeed;

        ref TrsComp ownerTrs = ref owner.Get<TrsComp>();
        Vector2 delta = ownerTrs.position - trs.position;
        if (delta.LengthSquared() < boomrConfig.maxDist * boomrConfig.maxDist)
            return;

        // if bullet moves towards player
        if (Vector2.Dot(delta, rigid.velocity) > 0.0f)
            return;

        delta = Vector2.Normalize(delta);
        float speed = rigid.velocity.Length();
        rigid.velocity = delta * speed;
    }
}
