using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Other;
using Components.Physics;
using Systems;
using Utils;

namespace Weapons.Specific;

class Book : BulletWeapon
{
    public Book(
        BulletConfig bulletConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context
    )
        : base(bulletConfig, config, callbacks, context) { }

    protected override void OnTimer(
        Entity entity,
        Entity? extra,
        ref ModComp modComp,
        Vector2 position
    )
    {
        float step = MathF.Tau / config.maxEnemies;
        for (int i = 0; i < config.maxEnemies; i++)
        {
            float angle = i * step;
            Vector2 dir = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
            float dist = (0.3f + 0.7f * Random.Shared.NextSingle()) * config.detectRadius;
            InstantiateBullet(entity, extra, ref modComp, position + dir * dist, Vector2.Zero);
        }
    }

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
        if (timer.time > 0.0f)
            return;

        ref ModComp mod = ref owner.Get<ModComp>();

        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        context.spatialSys.GetOverlap(
            owner,
            trs.position,
            coll.radius,
            config.targetLayer,
            overlap
        );
        for (int i = 0; i < overlap.Count; i++)
            Damage(owner, ref mod, overlap[i]);
    }
}
