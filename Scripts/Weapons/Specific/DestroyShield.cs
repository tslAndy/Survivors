using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Other;
using Systems;
using Utils;

namespace Weapons.Specific;

public class DestroyShield : Shield
{
    public DestroyShield(ShieldConfig config, ShieldCallbacks callbacks, WorldContext context)
        : base(config, callbacks, context) { }

    protected override void OnUpdate(
        Entity entity,
        Entity? extra,
        ref ModComp mod,
        Vector2 position,
        float dt
    )
    {
        using CachedList<Entity> overlap = CachedList<Entity>.Create();

        float radius = mod[DetectRadiusHash] * config.detectRadius;
        context.spatialSys.GetOverlap(entity, position, radius, config.projectileLayer, overlap);
        for (int i = 0; i < overlap.Count; i++)
        {
            Entity bullet = overlap[i];
            ref TimerComp timer = ref bullet.Get<TimerComp>();
            timer.time = 0.0f;
        }
    }
}
