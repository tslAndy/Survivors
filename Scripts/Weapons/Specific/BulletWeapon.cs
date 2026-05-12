using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Fighting;
using Components.Other;
using Components.Physics;
using Engine.Animations;
using Engine.Common;
using Engine.Sprites;
using Systems;
using Systems.Basic;
using Systems.Physics;
using Utils;

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

class BulletWeapon : Weapon, IBulletWeapon
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

    protected override void OnTimer(
        Entity entity,
        Entity? extra,
        ref ModComp modComp,
        Vector2 position
    )
    {
        PlayAttackSound();

        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        context.spatialSys.GetOverlap(
            entity,
            position,
            config.detectRadius * modComp[DetectRadiusHash],
            config.targetLayer,
            overlap
        );
        for (int i = 0; i < overlap.Count; i++)
        {
            Vector2 enemyPos = overlap[i].Get<TrsComp>().position;
            InstantiateBullet(
                entity,
                ref modComp,
                position,
                Vector2.Normalize(enemyPos - position)
            );
        }
    }

    public void UpdateBullet(
        Entity entity,
        Entity bullet,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref CollComp coll
    )
    {
        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        context.spatialSys.GetOverlap(
            bullet,
            trs.position,
            coll.radius,
            config.targetLayer,
            overlap
        );

        ref ModComp modComp = ref entity.Get<ModComp>();

        if (bulletConfig.perforate)
        {
            for (int i = 0; i < overlap.Count; i++)
            {
                Entity enemy = overlap[i];
                if (
                    context.spatialSys.collRegistry.AddColl(bullet, enemy)
                    == CollisionRegistry.CollState.Enter
                )
                    Damage(entity, ref modComp, enemy);
            }
        }
        else
        {
            for (int i = 0; i < overlap.Count; i++)
                Damage(entity, ref modComp, overlap[i]);

            if (overlap.Count != 0)
            {
                context.commandBuffer.Destroy(bullet);
                return;
            }
        }

        using CachedList<TileColl> tileOverlap = CachedList<TileColl>.Create();
        context.tileSys.GetOverlap(
            trs.position,
            coll.radius,
            context.layerMap["Walls"],
            tileOverlap
        );

        if (bulletConfig.bounce)
        {
            if (tileOverlap.Count == 0)
                return;

            ref TileColl tileColl = ref tileOverlap[0];
            trs.position += 1.0001f * tileColl.depth * tileColl.normal;
            rigid.velocity = Vector2.Reflect(rigid.velocity, tileColl.normal);
            trs.rotation = Single.RadiansToDegrees(MathF.Atan2(rigid.velocity.Y, rigid.velocity.X));
        }
        else if (tileOverlap.Count != 0)
        {
            context.commandBuffer.Destroy(bullet);
        }
    }

    private void InstantiateBullet(
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
            velocity = bulletConfig.velocity * direction * modComp[BulletSpeedHash],
        };
        CollComp coll = new CollComp { radius = bulletConfig.radius };
        BulletComp bullet = new BulletComp { owner = owner, weapon = this };
        TimerDestroyComp destroyComp = new TimerDestroyComp { time = bulletConfig.lifetime };

        SpriteComp sprite = new SpriteComp { drawOrder = bulletConfig.drawOrder };
        if (bulletConfig.sprite != null)
        {
            sprite.sprite = bulletConfig.sprite;

            context.world.Create<
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
            context.world.Create<
                AnimComp,
                SpriteComp,
                TrsComp,
                RigidComp,
                CollComp,
                BulletComp,
                TimerDestroyComp
            >(anim, sprite, trs, rigid, coll, bullet, destroyComp);
        }
    }
}
