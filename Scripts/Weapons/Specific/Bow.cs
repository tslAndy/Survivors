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
        int targetLayer
    )
        : base(bulletConfig, config, callbacks, targetLayer) { }

    protected override void OnTimer(Entity entity, Vector2 position)
    {
        using CachedList<Entity> entityOverlap = CachedList<Entity>.Create();
        spatial.GetOverlap(entity, position, config.detectRadius, targetLayer, entityOverlap);
        for (int i = 0; i < entityOverlap.Count; i++)
        {
            ref TransformComp enemyTrs = ref entityOverlap[i].Get<TransformComp>();
            InstantiateBullet(entity, position, Vector2.Normalize(enemyTrs.position - position));
        }
    }

    public override void UpdateBullet(Entity owner, Entity bullet, Vector2 position, float radius)
    {
        using CachedList<Entity> entityOverlap = CachedList<Entity>.Create();
        spatial.GetOverlap(bullet, position, radius, targetLayer, entityOverlap);

        for (int i = 0; i < entityOverlap.Count; i++)
            Damage(owner, entityOverlap[i]);

        if (entityOverlap.Count != 0)
        {
            commandBuffer.Destroy(bullet);
        }
        else
        {
            using CachedList<TileColl> tileOverlap = CachedList<TileColl>.Create();
            tileCollSys.GetOverlap(position, radius, default, tileOverlap);
            if (tileOverlap.Count != 0)
                commandBuffer.Destroy(bullet);
        }
    }
}
