using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;

partial class StatusEffectSys : BaseSystem<World, float>
{
    private const float APPLY_TIME = 1.0f; // all effects are applied 400 ms
    private const float MAX_DURATION = 5.0f;
    private const int MAX_DPS = 100;

    public StatusEffectSys(World world)
        : base(world) { }

    [Query]
    [None(typeof(DeathComp))]
    private void ApplyNewEffects(ref StatusEffectComp effects)
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
                continue;
            }

            ref StatusEffect oldEff = ref effects.runningEffects[index];
            oldEff.duration = Math.Clamp(oldEff.duration + newEff.duration, 0.0f, MAX_DURATION);
            oldEff.val = Math.Clamp(oldEff.val + newEff.val, 0, MAX_DPS);
        }

        effects.newEffects.Reset();
    }

    [Query]
    [None(typeof(DeathComp))]
    private void UpdateEffects(
        [Data] in float dt,
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
            int effDamage = (int)MathF.Floor(effect.val * APPLY_TIME);
            damage.hits.Add(new Hit(effDamage, effect.type));

            effect.duration -= APPLY_TIME;
            if (effect.duration < 0.0f)
                effects.runningEffects.SwapRemove(i);
            else
                i++;
        }
    }
}
