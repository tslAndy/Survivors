using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Physics;
using Systems;
using Systems.Physics;
using Utils;

namespace Weapons.Specific;

struct SpinConfig
{
    public float rotSpeed,
        circleRadius;
    public int bulletsCount;
}

class SpinWeapon : BulletWeapon
{
    protected readonly SpinConfig spinConfig;

    public SpinWeapon(
        SpinConfig spinConfig,
        BulletConfig bulletConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context
    )
        : base(bulletConfig, config, callbacks, context)
    {
        this.spinConfig = spinConfig;
    }

    protected override void OnTimer(
        Entity entity,
        Entity? extra,
        ref ModComp modComp,
        Vector2 position
    ) => PlayAttackSound();

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

    public override void UpdateBullet(
        Entity owner,
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

    private void Instantiate(Entity owner, ref TrsComp centerTrs)
    {
        ref ModComp mod = ref owner.Get<ModComp>();

        for (int i = 0; i < spinConfig.bulletsCount; i++)
        {
            float angle = MathF.Tau / spinConfig.bulletsCount * i;
            Vector2 dir = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
            Vector2 position = spinConfig.circleRadius * dir;
            LocalTrsComp localTrs = new LocalTrsComp { scale = 1.0f, position = position };

            Entity bullet = InstantiateBullet(owner, ref mod, Vector2.Zero, Vector2.Zero);
            bullet.Add<LocalTrsComp>(localTrs);

            centerTrs.descs.Add(bullet);
        }
    }
}
