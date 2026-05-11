using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;

partial class StatusEffectSys : BaseSystem<World, float>
{
    private StatusEffectHandler _effectHandler;
    private const float APPLY_TIME = 1.0f;

    public StatusEffectSys(World world, StatusEffectHandler effectHandler)
        : base(world)
    {
        this._effectHandler = effectHandler;
    }

    [Query]
    [None(typeof(DeathComp))]
    private void ApplyNewEffects(Entity entity, ref StatusEffectComp effects)
    {
        for (int i = 0; i < effects.newEffects.Count; i++)
        {
            ref StatusEffect newEff = ref effects.newEffects[i];
            int index = effects.runningEffects.IndexOf(
                (eff, type) => eff.type == type,
                newEff.type
            );

            if (index < 0)
            {
                effects.runningEffects.Add(newEff);
                _effectHandler.AddEffect(entity, ref newEff);
                continue;
            }

            ref StatusEffect oldEff = ref effects.runningEffects[index];

            float oldVal = oldEff.val;
            _effectHandler.CombineEffects(entity, ref oldEff, ref newEff);
        }

        effects.newEffects.Reset();
    }

    [Query]
    [None(typeof(DeathComp))]
    private void UpdateEffects(
        [Data] in float dt,
        Entity entity,
        ref StatusEffectComp effects,
        ref DamageComp damage
    )
    {
        effects.time += dt;
        if (effects.time < APPLY_TIME)
            return;

        effects.time -= APPLY_TIME;

        int i = 0;
        while (i < effects.runningEffects.Count)
        {
            ref StatusEffect effect = ref effects.runningEffects[i];

            // apply hit if effect is simple
            if (LongStatEffType.SimpleEffects.CheckFlag(effect.type))
            {
                int effDamage = (int)MathF.Floor(effect.val * APPLY_TIME);
                damage.hits.Add(new Hit(effDamage, effect.type));
            }

            effect.duration -= APPLY_TIME;
            if (effect.duration < 0.0f)
            {
                effects.runningEffects.SwapRemove(i);
                _effectHandler.RemoveEffect(entity, ref effect);
            }
            else
            {
                i++;
            }
        }
    }

    [Query]
    private void HandleDeath(in DeathComp death, ref StatusEffectComp effects)
    {
        if (!death.isDead)
            return;

        effects.newEffects.Dispose();
        effects.runningEffects.Dispose();
    }
}
