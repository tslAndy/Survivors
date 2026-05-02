using System.Numerics;
using Arch.Core;
using Components.Basic;
using Components.Fighting;
using Components.Physics;
using Engine.Sprites;

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

    protected BulletWeapon(
        BulletConfig bulletConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WeaponContext context
    )
        : base(config, callbacks, context)
    {
        this.bulletConfig = bulletConfig;
    }

    public abstract void UpdateBullet(Entity owner, Entity bullet, Vector2 position, float radius);

    protected void InstantiateBullet(Entity owner, Vector2 position, Vector2 direction)
    {
        context.world.Create<BulletComp, RigidComp, CollComp, TransformComp, SpriteComp>(
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
