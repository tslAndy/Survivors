using System.Numerics;
using Arch.Core;
using Components.Basic;
using Components.Fighting;
using Components.Physics;
using Engine.Animations;
using Engine.Sprites;
using Systems;

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
        Entity bullet,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref CollComp coll
    );

    protected void InstantiateBullet(Entity owner, Vector2 position, Vector2 direction)
    {
        TrsComp trs = new TrsComp
        {
            position = position,
            rotation = Single.RadiansToDegrees(MathF.Atan2(direction.Y, direction.X)),
            scale = 1.0f,
        };

        RigidComp rigid = new RigidComp { velocity = bulletConfig.velocity * direction };
        CollComp coll = new CollComp { radius = bulletConfig.radius };
        BulletComp bullet = new BulletComp { owner = owner, weapon = this };

        AnimComp anim = new AnimComp { timeScale = 1.0f };
        SpriteComp sprite = new SpriteComp { drawOrder = bulletConfig.drawOrder };
        if (bulletConfig.sprite != null)
            sprite.sprite = bulletConfig.sprite;
        else if (bulletConfig.anim != null)
            anim.anim = bulletConfig.anim;

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
