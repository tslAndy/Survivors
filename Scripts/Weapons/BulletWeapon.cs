using System.Numerics;
using Arch.Core;
using Components.Basic;
using Components.Fighting;
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
        radius;
    public int bulletLayer,
        drawOrder;
}

abstract class BulletWeapon : Weapon, IBulletWeapon
{
    protected readonly BulletConfig bulletConfig;
    protected readonly Hash bulletSpeedFactor;

    public BulletWeapon(
        BulletConfig bulletConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context,
        ModRegistry modRegistry
    )
        : base(config, callbacks, context, modRegistry)
    {
        this.bulletConfig = bulletConfig;
        this.bulletSpeedFactor = modRegistry["bulletSpeedFactor"];
    }

    public abstract void UpdateBullet(
        Entity owner,
        Entity bullet,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref CollComp coll
    );

    protected void InstantiateBullet(
        Entity owner,
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
            velocity = bulletConfig.velocity * direction * modComp[bulletSpeedFactor],
        };
        CollComp coll = new CollComp { radius = bulletConfig.radius };
        BulletComp bullet = new BulletComp { owner = owner, weapon = this };

        SpriteComp sprite = new SpriteComp { drawOrder = bulletConfig.drawOrder };
        if (bulletConfig.sprite != null)
        {
            sprite.sprite = bulletConfig.sprite;

            context.world.Create<SpriteComp, TrsComp, RigidComp, CollComp, BulletComp>(
                sprite,
                trs,
                rigid,
                coll,
                bullet
            );
        }
        else if (bulletConfig.anim != null)
        {
            AnimComp anim = new AnimComp { anim = bulletConfig.anim };
            context.world.Create<AnimComp, SpriteComp, TrsComp, RigidComp, CollComp, BulletComp>(
                anim,
                sprite,
                trs,
                rigid,
                coll,
                bullet
            );
        }
    }
}
