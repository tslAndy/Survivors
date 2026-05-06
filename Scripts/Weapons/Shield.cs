using System.Numerics;
using Arch.Core;
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

class Shield
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
        ref DamageComp damage,
        ref StatusEffectComp effects,
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

        OnUpdate(entity, position, dt);
    }

    protected virtual void OnUpdate(Entity entity, Vector2 position, float dt) { }
}
