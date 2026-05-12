using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Fighting;
using Components.Physics;
using Engine.Animations;
using Engine.Sprites;
using Systems;
using Systems.Physics;
using Utils;

namespace Weapons.Specific;

struct SpinConfig
{
    public Anim? anim;
    public Sprite? sprite;

    public float rotSpeed,
        circleRadius,
        bulletRadius;
    public int bulletsCount,
        drawOrder;
}

class SpinWeapon : Weapon, ISpinWeapon
{
    protected readonly SpinConfig spinConfig;

    public SpinWeapon(
        SpinConfig spinConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context
    )
        : base(config, callbacks, context)
    {
        this.spinConfig = spinConfig;
    }

    public void UpdateBullet(Entity owner, Entity bullet, ref TrsComp globalTrs, ref CollComp coll)
    {
        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        context.spatialSys.GetOverlap(
            bullet,
            globalTrs.position,
            coll.radius,
            config.targetLayer,
            overlap
        );

        ref ModComp mod = ref owner.Get<ModComp>();

        for (int i = 0; i < overlap.Count; i++)
        {
            Entity enemy = overlap[i];
            if (
                context.spatialSys.collRegistry.AddColl(bullet, enemy)
                == CollisionRegistry.CollState.Enter
            )
                Damage(owner, ref mod, enemy);
        }
    }

    protected override void OnUpdate(
        Entity entity,
        Entity? extra,
        ref ModComp modComp,
        Vector2 position,
        float dt
    )
    {
        if (extra == null)
            return;

        // if bullet weren't initialized
        ref TrsComp trs = ref extra.Value.Get<TrsComp>();
        if (trs.descs.Count == 0)
            Instantiate(entity, ref trs);

        ref LocalTrsComp localTrs = ref extra.Value.Get<LocalTrsComp>();
        localTrs.rotation += dt * spinConfig.rotSpeed;
    }

    private void Instantiate(Entity owner, ref TrsComp centerTrs)
    {
        for (int i = 0; i < spinConfig.bulletsCount; i++)
        {
            float angle = MathF.Tau / spinConfig.bulletsCount * i;
            Vector2 dir = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
            Vector2 position = spinConfig.circleRadius * dir;

            TrsComp globalTrs = new TrsComp { scale = 1.0f };
            LocalTrsComp localTrs = new LocalTrsComp { scale = 1.0f, position = position };
            CollComp coll = new CollComp { radius = spinConfig.bulletRadius };
            SpriteComp sprite = new SpriteComp { drawOrder = spinConfig.drawOrder };
            SpinComp spin = new SpinComp { owner = owner, weapon = this };

            if (spinConfig.sprite != null)
            {
                sprite.sprite = spinConfig.sprite;

                Entity bullet = context.world.Create<
                    SpriteComp,
                    TrsComp,
                    LocalTrsComp,
                    CollComp,
                    SpinComp
                >(sprite, globalTrs, localTrs, coll, spin);
                centerTrs.descs.Add(bullet);
            }
            else if (spinConfig.anim != null)
            {
                AnimComp anim = new AnimComp { anim = spinConfig.anim };
                Entity bullet = context.world.Create<
                    AnimComp,
                    SpriteComp,
                    TrsComp,
                    LocalTrsComp,
                    CollComp,
                    SpinComp
                >(anim, sprite, globalTrs, localTrs, coll, spin);
                centerTrs.descs.Add(bullet);
            }
        }
    }
}
