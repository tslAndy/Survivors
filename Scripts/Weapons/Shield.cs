using System.Numerics;
using Arch.Core;
using Components.Basic;
using Components.Fighting;
using Components.Health;
using Engine.Common;
using Systems;
using Systems.Basic;

namespace Weapons;

public struct ShieldCallbacks
{
    public HitCallback? hitCallback;
    public EffectCallback? runningEffectCallback,
        newEffectCallback;

    public delegate void HitCallback(Entity entity, ref Hit hit);
    public delegate void EffectCallback(Entity entity, ref StatusEffect effect);
}

public struct ShieldConfig
{
    public float detectRadius;
    public int entityLayer,
        projectileLayer;
}

public class Shield : IShield
{
    protected readonly ShieldConfig config;
    protected readonly ShieldCallbacks callbacks;
    protected readonly WorldContext context;

    protected readonly Hash DetectRadiusHash = ModRegistry.CountHash("detectRadiusFactor");

    public Shield(ShieldConfig config, ShieldCallbacks callbacks, WorldContext context)
    {
        this.config = config;
        this.callbacks = callbacks;
        this.context = context;
    }

    public void Update(
        Entity entity,
        Entity? extra,
        ref DamageComp damage,
        ref StatusEffectComp effects,
        ref ModComp mod,
        Vector2 position,
        float dt
    )
    {
        if (callbacks.hitCallback != null)
        {
            for (int i = 0; i < damage.hits.Count; i++)
                callbacks.hitCallback(entity, ref damage.hits[i]);
        }

        if (callbacks.runningEffectCallback != null)
        {
            for (int i = 0; i < effects.runningEffects.Count; i++)
                callbacks.runningEffectCallback(entity, ref effects.runningEffects[i]);
        }

        if (callbacks.newEffectCallback != null)
        {
            for (int i = 0; i < effects.newEffects.Count; i++)
                callbacks.newEffectCallback(entity, ref effects.newEffects[i]);
        }

        OnUpdate(entity, extra, ref mod, position, dt);
    }

    protected virtual void OnUpdate(
        Entity entity,
        Entity? extra,
        ref ModComp mod,
        Vector2 position,
        float dt
    ) { }
}
