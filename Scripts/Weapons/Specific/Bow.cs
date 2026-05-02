using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Systems.Physics;
using Utils;

namespace Weapons.Specific;

class Bow : BulletWeapon
{
    public Bow(
        BulletConfig bulletConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WeaponContext context
    )
        : base(bulletConfig, config, callbacks, context) { }

    protected override void OnTimer(Entity entity, Vector2 position)
    {
        using CachedList<Entity> entityOverlap = CachedList<Entity>.Create();
        context.spatial.GetOverlap(
            entity,
            position,
            config.detectRadius,
            config.targetLayer,
            entityOverlap
        );
        for (int i = 0; i < entityOverlap.Count; i++)
        {
            ref TransformComp enemyTrs = ref entityOverlap[i].Get<TransformComp>();
            InstantiateBullet(entity, position, Vector2.Normalize(enemyTrs.position - position));
        }
    }

    public override void UpdateBullet(Entity owner, Entity bullet, Vector2 position, float radius)
    {
        using CachedList<Entity> entityOverlap = CachedList<Entity>.Create();
        context.spatial.GetOverlap(bullet, position, radius, config.targetLayer, entityOverlap);

        for (int i = 0; i < entityOverlap.Count; i++)
            Damage(owner, entityOverlap[i]);

        if (entityOverlap.Count != 0)
        {
            context.commandBuffer.Destroy(bullet);
        }
        else
        {
            using CachedList<TileColl> tileOverlap = CachedList<TileColl>.Create();
            context.tileCollSys.GetOverlap(
                position,
                radius,
                context.layerMap["Walls"],
                tileOverlap
            );
            if (tileOverlap.Count != 0)
                context.commandBuffer.Destroy(bullet);
        }
    }
}
