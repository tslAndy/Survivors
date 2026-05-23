using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Other;
using Components.Physics;
using Systems;
using Systems.Physics;
using Utils;

namespace Weapons.Specific;

class Bow : BulletWeapon
{
    public Bow(
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
        PlayAttackSound();

        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        context.spatialSys.GetOverlap(
            entity,
            position,
            config.detectRadius * modComp[DetectRadiusHash],
            config.targetLayer,
            overlap
        );

        int n = Math.Min(overlap.Count, config.maxEnemies);
        while (n > 0)
        {
            Vector2 enemyPos = overlap.RandPop().Get<TrsComp>().position;
            InstantiateBullet(
                entity,
                extra,
                ref modComp,
                position,
                Vector2.Normalize(enemyPos - position)
            );
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
        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        context.spatialSys.GetOverlap(
            bullet,
            trs.position,
            coll.radius,
            config.targetLayer,
            overlap
        );

        ref ModComp modComp = ref owner.Get<ModComp>();
        if (bulletConfig.perforate)
        {
            for (int i = 0; i < overlap.Count; i++)
            {
                Entity enemy = overlap[i];
                if (
                    context.spatialSys.collRegistry.AddColl(bullet, enemy)
                    == CollisionRegistry.CollState.Enter
                )
                    Damage(owner, ref modComp, enemy);
            }
        }
        else
        {
            for (int i = 0; i < overlap.Count; i++)
                Damage(owner, ref modComp, overlap[i]);

            if (overlap.Count != 0)
            {
                timer.time = 0.0f;
                return;
            }
        }

        using CachedList<TileColl> tileOverlap = CachedList<TileColl>.Create();
        context.tileSys.GetOverlap(
            trs.position,
            coll.radius,
            context.layerMap["Walls"],
            tileOverlap
        );

        if (bulletConfig.bounce)
        {
            if (tileOverlap.Count == 0)
                return;

            ref TileColl tileColl = ref tileOverlap[0];
            trs.position += 1.0001f * tileColl.depth * tileColl.normal;
            rigid.velocity = Vector2.Reflect(rigid.velocity, tileColl.normal);
            trs.rotation = Single.RadiansToDegrees(MathF.Atan2(rigid.velocity.Y, rigid.velocity.X));
        }
        else if (tileOverlap.Count != 0)
        {
            timer.time = 0.0f;
        }
    }
}
