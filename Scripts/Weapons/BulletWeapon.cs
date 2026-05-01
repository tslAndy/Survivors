using System.Numerics;
using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Fighting;
using Components.Physics;
using Engine.Sprites;
using Systems.Physics;
using Utils;

namespace Weapons;

struct BulletConfig
{
    public int layer;
    public float radius,
        speed;
    public Sprite sprite;
}

abstract class BulletWeapon : Weapon
{
    protected readonly BulletConfig bulletConfig;

    protected readonly World world;
    protected readonly CommandBuffer commandBuffer;
    protected readonly TileCollSys tileCollSys;

    protected BulletWeapon(
        BulletConfig bulletConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        int targetLayer
    )
        : base(config, callbacks, targetLayer)
    {
        this.bulletConfig = bulletConfig;

        this.world = ServiceLocator.Get<World>();
        this.commandBuffer = ServiceLocator.Get<CommandBuffer>();
        this.tileCollSys = ServiceLocator.Get<TileCollSys>();
    }

    public abstract void UpdateBullet(Entity owner, Entity bullet, Vector2 position, float radius);

    protected void InstantiateBullet(Entity owner, Vector2 position, Vector2 direction)
    {
        world.Create<BulletComp, RigidComp, CollComp, TransformComp, SpriteComp>(
            new BulletComp { owner = owner, weapon = this },
            new RigidComp { velocity = direction * bulletConfig.speed, layer = bulletConfig.layer },
            new CollComp { radius = bulletConfig.radius },
            new TransformComp
            {
                position = position,
                scale = 1.0f,
                rotation = Single.RadiansToDegrees(MathF.Atan2(direction.Y, direction.X)),
            },
            new SpriteComp { sprite = bulletConfig.sprite, drawOrder = 1 }
        );
    }
}

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
        CachedList<Entity> entityOverlap = ObjectPool<CachedList<Entity>>.Shared.Get();
        spatial.GetOverlap(entity, position, config.detectRadius, targetLayer, entityOverlap);

        for (int i = 0; i < entityOverlap.Count; i++)
        {
            ref TransformComp enemyTrs = ref entityOverlap[i].Get<TransformComp>();
            InstantiateBullet(entity, position, Vector2.Normalize(enemyTrs.position - position));
        }

        ObjectPool<CachedList<Entity>>.Shared.Return(entityOverlap);
    }

    public override void UpdateBullet(Entity owner, Entity bullet, Vector2 position, float radius)
    {
        CachedList<Entity> entityOverlap = ObjectPool<CachedList<Entity>>.Shared.Get();
        spatial.GetOverlap(bullet, position, radius, targetLayer, entityOverlap);

        for (int i = 0; i < entityOverlap.Count; i++)
            Damage(owner, entityOverlap[i]);

        if (entityOverlap.Count != 0)
        {
            commandBuffer.Destroy(bullet);
        }
        else
        {
            CachedList<TileColl> tileOverlap = ObjectPool<CachedList<TileColl>>.Shared.Get();
            tileCollSys.GetOverlap(position, radius, default, tileOverlap);
            if (tileOverlap.Count != 0)
                commandBuffer.Destroy(bullet);
            ObjectPool<CachedList<TileColl>>.Shared.Return(tileOverlap);
        }

        ObjectPool<CachedList<Entity>>.Shared.Return(entityOverlap);
    }
}
