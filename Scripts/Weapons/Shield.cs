using System.Numerics;
using Arch.Core;
using Components.Basic;
using Components.Fighting;
using Systems;

namespace Weapons;

struct ShieldCallbacks
{
    public HitCallback? hitCallback;
    public EffectCallback? effectCallback;

    public delegate void HitCallback(Entity entity, ref Hit hit);
    public delegate void EffectCallback(Entity entity, ref StatusEffect effect);
}

class Shield : IShield
{
    protected readonly ShieldCallbacks callbacks;
    protected readonly WorldContext context;

    public Shield(ShieldCallbacks callbacks, WorldContext context)
    {
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

        if (callbacks.effectCallback != null)
        {
            for (int i = 0; i < effects.newEffects.Count; i++)
                callbacks.effectCallback(entity, ref effects.newEffects[i]);
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
