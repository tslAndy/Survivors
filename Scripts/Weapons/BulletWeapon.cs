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
        ref TransformComp trs,
        ref RigidComp rigid,
        ref CollComp coll
    );

    protected abstract void InstantiateBullet(Entity owner, Vector2 position, Vector2 direction);
}
