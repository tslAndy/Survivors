using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Components.Basic;
using Components.Characters;
using Components.Fighting;
using Components.Loot;

partial class StatusEffectSys : BaseSystem<World, float>
{
    private StatusEffectHandler _effectHandler;
    private const float APPLY_TIME = 1.0f;

    public StatusEffectSys(StatusEffectHandler effectHandler, World world)
        : base(world)
    {
        _effectHandler = effectHandler;
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
                _effectHandler.OnEffectAdd(entity, ref newEff);
                continue;
            }

            ref StatusEffect oldEff = ref effects.runningEffects[index];
            _effectHandler.CombineEffects(ref oldEff, ref newEff);
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
                _effectHandler.OnEffectRemove(entity, ref effect);
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

class StatusEffectHandler
{
    private const float MAX_DURATION = 5.0f;
    private const float MAX_DPS = 100;

    public void CombineEffects(ref StatusEffect first, ref StatusEffect second)
    {
        if (LongStatEffType.SimpleEffects.CheckFlag(first.type))
        {
            first.duration = Math.Clamp(first.duration + second.duration, 0.0f, MAX_DURATION);
            first.val = Math.Clamp(first.val + second.val, 0.0f, MAX_DPS);
        }
        // effect is complex
        else if (first.type != StatusEffectType.Curse)
        {
            first.duration = Math.Clamp(first.duration + second.duration, 0.0f, MAX_DURATION);
        }
    }

    public void OnEffectAdd(Entity entity, ref StatusEffect effect)
    {
        switch (effect.type)
        {
            case StatusEffectType.Armor:
            case StatusEffectType.Sensitivity:
                entity.Get<DamageComp>().damageFactor = effect.val;
                break;

            case StatusEffectType.Weaken:
            case StatusEffectType.Rage:
                ref WeaponComp weapon = ref entity.Get<WeaponComp>();
                weapon.dpsFactor = effect.val;
                for (int i = 0; i < weapon.weapons.Count; i++)
                    weapon.weapons[i].entity?.Get<AnimComp>().timeScale = effect.val;

                ref ShieldComp shield = ref entity.Get<ShieldComp>();
                shield.dpsFactor = effect.val;
                for (int i = 0; i < shield.shields.Count; i++)
                    shield.shields[i].entity?.Get<AnimComp>().timeScale = effect.val;

                break;

            case StatusEffectType.Haste:
            case StatusEffectType.Slowness:
                entity.Get<CharMoveComp>().speedFactor = effect.val;
                break;

            case StatusEffectType.Greed:
            case StatusEffectType.Poverty:
                entity.Get<LootCollComp>().incomeFactor = effect.val;
                break;

            case StatusEffectType.ShortHand:
            case StatusEffectType.LongHand:
                entity.Get<LootCollComp>().radiusFactor = effect.val;
                break;

            default:
                break;
        }
    }

    public void OnEffectRemove(Entity entity, ref StatusEffect effect)
    {
        switch (effect.type)
        {
            case StatusEffectType.Armor:
            case StatusEffectType.Sensitivity:
                entity.Get<DamageComp>().damageFactor = 1.0f;
                break;

            case StatusEffectType.Weaken:
            case StatusEffectType.Rage:
                ref WeaponComp weapon = ref entity.Get<WeaponComp>();
                weapon.dpsFactor = effect.val;
                for (int i = 0; i < weapon.weapons.Count; i++)
                    weapon.weapons[i].entity?.Get<AnimComp>().timeScale = 1.0f;

                ref ShieldComp shield = ref entity.Get<ShieldComp>();
                shield.dpsFactor = effect.val;
                for (int i = 0; i < shield.shields.Count; i++)
                    shield.shields[i].entity?.Get<AnimComp>().timeScale = 1.0f;

                break;

            case StatusEffectType.Haste:
            case StatusEffectType.Slowness:
                entity.Get<CharMoveComp>().speedFactor = 1.0f;
                break;

            case StatusEffectType.Curse:
                entity.Get<DamageComp>().hits.Add(new Hit(1_000_000, StatusEffectType.Curse));
                break;

            case StatusEffectType.Greed:
            case StatusEffectType.Poverty:
                entity.Get<LootCollComp>().incomeFactor = 1.0f;
                break;

            case StatusEffectType.ShortHand:
            case StatusEffectType.LongHand:
                entity.Get<LootCollComp>().radiusFactor = 1.0f;
                break;

            default:
                break;
        }
    }
}
