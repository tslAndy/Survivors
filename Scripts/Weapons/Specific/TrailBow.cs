using Arch.Core;
using Components.Basic;
using Components.Physics;
using Raylib_cs;
using Systems;

namespace Weapons.Specific;

struct TrailConfig
{
    public float length,
        thick;
    public Color color;
}

class TrailBow : Bow
{
    protected readonly TrailConfig trailConfig;

    public TrailBow(
        TrailConfig trailConfig,
        BulletConfig bulletConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context
    )
        : base(bulletConfig, config, callbacks, context)
    {
        this.trailConfig = trailConfig;
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
    }
}
