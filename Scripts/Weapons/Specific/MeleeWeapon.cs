using System.Numerics;
using Arch.Core;
using Components.Basic;
using Systems;
using Utils;

namespace Weapons.Specific;

class MeleeWeapon : Weapon
{
    public MeleeWeapon(WeaponConfig config, WeaponCallbacks callbacks, WorldContext context)
        : base(config, callbacks, context) { }

    protected override void OnUpdate(
        Entity entity,
        Entity? extra,
        ref ModComp modComp,
        Vector2 position,
        float dt
    ) { }

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
            Damage(entity, ref modComp, overlap[i]);
    }
}
