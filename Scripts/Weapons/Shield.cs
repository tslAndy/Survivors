using System.Numerics;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Utils;

namespace Weapons;

struct ShieldCallbacks
{
    public HitCallback? hitCallback;
    public EffectCallback? effectCallback;

    public delegate void HitCallback(Entity entity, ref Hit hit);
    public delegate void EffectCallback(Entity entity, ref StatusEffect effect);
}

struct ShieldComp
{
    public CachedList<Shield> shields;
}

class Shield
{
    protected readonly ShieldCallbacks callbacks;

    public Shield(ShieldCallbacks callbacks)
    {
        this.callbacks = callbacks;
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

partial class ShieldSys : BaseSystem<World, float>
{
    public ShieldSys(World world)
        : base(world) { }

    [Query]
    private void UpdateShield(
        [Data] in float dt,
        Entity entity,
        ref ShieldComp shield,
        ref TransformComp trs,
        ref DamageComp damage,
        ref StatusEffectComp effects
    )
    {
        for (int i = 0; i < shield.shields.Count; i++)
        {
            ref Shield cur = ref shield.shields[i];
            cur.Update(entity, ref damage, ref effects, trs.position, dt);
        }
    }
}
