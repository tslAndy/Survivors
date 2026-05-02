using System.Numerics;
using Arch.Buffer;
using Arch.Core;
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
