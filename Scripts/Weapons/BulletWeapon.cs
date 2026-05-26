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

public struct BulletConfig
{
    public Anim? anim;
    public Sprite? sprite;

    public float velocity,
        rotVelocity,
        radius,
        lifetime;

    public int bulletLayer,
        drawOrder;

    public bool perforate,
        bounce;
}

public abstract class BulletWeapon : Weapon, IBulletWeapon
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
        ref CollComp coll,
        ref TimerComp timer
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
            rotVelocity = bulletConfig.rotVelocity,
            layer = bulletConfig.bulletLayer,
        };

        BulletComp bullet = new BulletComp
        {
            owner = owner,
            extra = extra,
            weapon = this,
        };

        CollComp coll = new CollComp { radius = bulletConfig.radius };
        TimerComp destroyComp = new TimerComp { time = bulletConfig.lifetime };
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
                TimerComp
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
                TimerComp
            >(anim, sprite, trs, rigid, coll, bullet, destroyComp);
        }

        return context.world.Create<TrsComp, RigidComp, CollComp, BulletComp, TimerComp>(
            trs,
            rigid,
            coll,
            bullet,
            destroyComp
        );
    }
}
