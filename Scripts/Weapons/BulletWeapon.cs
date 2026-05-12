using System.Numerics;
using Arch.Core;
using Components.Basic;
using Components.Fighting;
using Components.Other;
using Components.Physics;
using Engine.Animations;
using Engine.Common;
using Engine.Sprites;
using Systems;
using Systems.Basic;

namespace Weapons;

struct BulletConfig
{
    public Anim? anim;
    public Sprite? sprite;

    public float velocity,
        radius,
        lifetime;

    public int bulletLayer,
        drawOrder;

    public bool perforate,
        bounce;
}

abstract class BulletWeapon : Weapon, IBulletWeapon
{
    protected readonly BulletConfig bulletConfig;
    protected readonly Hash BulletSpeedHash = ModRegistry.CountHash("bulletSpeedFactor");

    public BulletWeapon(
        BulletConfig bulletConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context
    )
        : base(config, callbacks, context)
    {
        this.bulletConfig = bulletConfig;
    }

    public abstract void UpdateBullet(
        Entity owner,
        Entity? extra,
        Entity bullet,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref CollComp coll
    );

    protected Entity InstantiateBullet(
        Entity owner,
        Entity? extra,
        ref ModComp modComp,
        Vector2 position,
        Vector2 direction
    )
    {
        TrsComp trs = new TrsComp
        {
            position = position,
            rotation = Single.RadiansToDegrees(MathF.Atan2(direction.Y, direction.X)),
            scale = 1.0f,
        };

        RigidComp rigid = new RigidComp
        {
            velocity = bulletConfig.velocity * direction * modComp[BulletSpeedHash],
        };

        CollComp coll = new CollComp { radius = bulletConfig.radius };
        BulletComp bullet = new BulletComp
        {
            owner = owner,
            extra = extra,
            weapon = this,
        };
        TimerDestroyComp destroyComp = new TimerDestroyComp { time = bulletConfig.lifetime };

        SpriteComp sprite = new SpriteComp { drawOrder = bulletConfig.drawOrder };
        if (bulletConfig.sprite != null)
        {
            sprite.sprite = bulletConfig.sprite;

            return context.world.Create<
                SpriteComp,
                TrsComp,
                RigidComp,
                CollComp,
                BulletComp,
                TimerDestroyComp
            >(sprite, trs, rigid, coll, bullet, destroyComp);
        }
        else if (bulletConfig.anim != null)
        {
            AnimComp anim = new AnimComp { anim = bulletConfig.anim };
            return context.world.Create<
                AnimComp,
                SpriteComp,
                TrsComp,
                RigidComp,
                CollComp,
                BulletComp,
                TimerDestroyComp
            >(anim, sprite, trs, rigid, coll, bullet, destroyComp);
        }

        throw new Exception();
    }
}
