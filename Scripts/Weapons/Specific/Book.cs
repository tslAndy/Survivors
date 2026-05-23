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

    protected override void OnTimer(Entity entity, Entity? extra, ref ModComp mod, Vector2 position)
    {
        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        context.spatialSys.GetOverlap(
            entity,
            position,
            config.detectRadius * mod[DetectRadiusHash],
            config.targetLayer,
            overlap
        );

        int n = Math.Min(overlap.Count, config.maxEnemies);
        while (n > 0)
        {
            ref TrsComp enemyTrs = ref overlap.RandPop().Get<TrsComp>();
            InstantiateBullet(entity, extra, ref mod, enemyTrs.position, Vector2.Zero);
            n--;
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
