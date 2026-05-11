using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Fighting;
using Components.Physics;
using Engine.Common;
using Systems;
using Systems.Basic;
using Systems.Physics;
using Utils;

namespace Weapons.Specific;

class Bow : BulletWeapon, IBulletWeapon
{
    public Bow(
        BulletConfig bulletConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context,
        ModRegistry modRegistry
    )
        : base(bulletConfig, config, callbacks, context, modRegistry) { }

    protected override void OnTimer(Entity entity, ref ModComp modComp, Vector2 position)
    {
        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        context.spatial.GetOverlap(
            entity,
            position,
            config.detectRadius * modComp[detectRadiusHash],
            config.targetLayer,
            overlap
        );
        for (int i = 0; i < overlap.Count; i++)
        {
            Vector2 enemyPos = overlap[i].Get<TrsComp>().position;
            InstantiateBullet(
                entity,
                ref modComp,
                position,
                Vector2.Normalize(enemyPos - position)
            );
        }
    }

    public override void UpdateBullet(
        Entity entity,
        Entity bullet,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref CollComp coll
    )
    {
        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        context.spatial.GetOverlap(entity, trs.position, coll.radius, config.targetLayer, overlap);

        ref ModComp modComp = ref entity.Get<ModComp>();

        for (int i = 0; i < overlap.Count; i++)
            Damage(entity, ref modComp, overlap[i]);

        if (overlap.Count != 0)
        {
            context.commandBuffer.Destroy(bullet);
            return;
        }

        using CachedList<TileColl> tileOverlap = CachedList<TileColl>.Create();
        context.tileCollSys.GetOverlap(
            trs.position,
            coll.radius,
            context.layerMap["Walls"],
            tileOverlap
        );
        if (tileOverlap.Count != 0)
            context.commandBuffer.Destroy(bullet);
    }
}
