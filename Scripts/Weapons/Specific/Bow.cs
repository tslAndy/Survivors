using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Fighting;
using Components.Physics;
using Systems;
using Systems.Physics;
using Utils;

namespace Weapons.Specific;

class Bow : BulletWeapon, IBulletWeapon
{
    public Bow(
        BulletConfig bulletConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context
    )
        : base(bulletConfig, config, callbacks, context) { }

    protected override void OnTimer(Entity entity, Vector2 position)
    {
        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        context.spatial.GetOverlap(
            entity,
            position,
            config.detectRadius,
            config.targetLayer,
            overlap
        );
        for (int i = 0; i < overlap.Count; i++)
        {
            Vector2 enemyPos = overlap[i].Get<TransformComp>().position;
            InstantiateBullet(entity, position, Vector2.Normalize(enemyPos - position));
        }
    }

    public override void UpdateBullet(
        Entity entity,
        Entity bullet,
        ref TransformComp trs,
        ref RigidComp rigid,
        ref CollComp coll
    )
    {
        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        context.spatial.GetOverlap(entity, trs.position, coll.radius, config.targetLayer, overlap);

        for (int i = 0; i < overlap.Count; i++)
            Damage(entity, overlap[i]);

        if (overlap.Count != 0)
        {
            context.commandBuffer.Destroy(bullet);
            return;
        }

        using CachedList<TileColl> tileOverlap = CachedList<TileColl>.Create();
        context.tileCollSys.GetOverlap(
            trs.position,
            coll.radius,
            context.layerMap["Walls"],
            tileOverlap
        );
        if (overlap.Count != 0)
            context.commandBuffer.Destroy(bullet);
    }

    protected override void InstantiateBullet(Entity entity, Vector2 position, Vector2 direction)
    {
        TransformComp trs = new TransformComp
        {
            position = position,
            rotation = Single.RadiansToDegrees(MathF.Atan2(direction.Y, direction.X)),
            scale = 1.0f,
        };

        RigidComp rigid = new RigidComp { velocity = bulletConfig.velocity * direction };
        CollComp coll = new CollComp { radius = bulletConfig.radius };
        BulletComp bullet = new BulletComp { owner = entity, weapon = this };

        AnimComp anim = new AnimComp { timeScale = 1.0f };
        SpriteComp sprite = new SpriteComp { drawOrder = bulletConfig.drawOrder };
        if (bulletConfig.sprite != null)
            sprite.sprite = bulletConfig.sprite;
        else if (bulletConfig.anim != null)
            anim.anim = bulletConfig.anim;

        context.world.Create<AnimComp, SpriteComp, TransformComp, RigidComp, CollComp, BulletComp>(
            anim,
            sprite,
            trs,
            rigid,
            coll,
            bullet
        );
    }
}
