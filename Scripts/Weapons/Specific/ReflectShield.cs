using System.Numerics;
using Arch.Core;
using Components.Basic;
using Components.Fighting;
using Components.Physics;
using Systems;
using Utils;

namespace Weapons.Specific;

public class ReflectShield : Shield
{
    private readonly IBulletWeapon _bulletWeapon;

    public ReflectShield(
        IBulletWeapon bulletWeapon,
        ShieldConfig config,
        ShieldCallbacks callbacks,
        WorldContext context
    )
        : base(config, callbacks, context)
    {
        _bulletWeapon = bulletWeapon;
    }

    protected override void OnUpdate(
        Entity entity,
        Entity? extra,
        ref ModComp mod,
        Vector2 position,
        float dt
    )
    {
        using CachedList<Entity> overlap = CachedList<Entity>.Create();

        float radius = mod[DetectRadiusHash] * config.detectRadius;
        context.spatialSys.GetOverlap(entity, position, radius, config.projectileLayer, overlap);
        for (int i = 0; i < overlap.Count; i++)
        {
            Entity bullet = overlap[i];
            Components<TrsComp, RigidComp, BulletComp> comps = bullet.Get<
                TrsComp,
                RigidComp,
                BulletComp
            >();
            ref TrsComp trs = ref comps.t0;
            ref RigidComp rigid = ref comps.t1;
            ref BulletComp bulletComp = ref comps.t2;

            if (Vector2.Dot(trs.position - position, rigid.velocity) > 0.0f)
                continue;

            rigid.velocity = -rigid.velocity;
            trs.rotation += 180.0f;

            bulletComp.owner = entity;
            bulletComp.weapon = _bulletWeapon;
            bulletComp.extra = null;
        }
    }
}
