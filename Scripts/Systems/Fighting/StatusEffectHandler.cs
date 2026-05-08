using Arch.Core;
using Arch.Core.Extensions;
using Components.Characters;
using Components.Fighting;
using Components.Loot;

class StatusEffectHandler
{
    private const float MAX_DURATION = 5.0f;
    private const float MAX_DPS = 100;

    public void CombineEffects(Entity entity, ref StatusEffect first, ref StatusEffect second)
    {
        switch (first.type)
        {
            // simple effects
            case StatusEffectType.Burn:
            case StatusEffectType.Bleed:
            case StatusEffectType.Poison:
            case StatusEffectType.Shock:
            case StatusEffectType.Regen:
                first.duration = Math.Clamp(first.duration + second.duration, 0.0f, MAX_DURATION);
                first.val = Math.Clamp(first.val + second.val, -MAX_DPS, MAX_DPS);
                break;

            // effect combined by max
            case StatusEffectType.Armor:
            case StatusEffectType.Delicacy:
            case StatusEffectType.Rage:
            case StatusEffectType.Haste:
            case StatusEffectType.Greed:
            case StatusEffectType.LongHand:
                float oldVal = first.val;
                first.duration = Math.Clamp(first.duration + second.duration, 0.0f, MAX_DURATION);
                first.val = Math.Max(first.val, second.val);
                MultiplyValue(entity, first.type, first.val / oldVal);
                break;

            // effects combined by min
            case StatusEffectType.Weaken:
            case StatusEffectType.Slowness:
            case StatusEffectType.Stuck:
            case StatusEffectType.Poverty:
            case StatusEffectType.ShortHand:
                oldVal = first.val;
                first.duration = Math.Clamp(first.duration + second.duration, 0.0f, MAX_DURATION);
                first.val = Math.Min(first.val, second.val);
                MultiplyValue(entity, first.type, first.val / oldVal);
                break;

            default:
                break;
        }
    }

    public void AddEffect(Entity entity, ref StatusEffect effect)
    {
        // non multiply effects logic handling if necessary
        MultiplyValue(entity, effect.type, effect.val);
    }

    public void RemoveEffect(Entity entity, ref StatusEffect effect)
    {
        // non multiply effects logic handling if necessary
        MultiplyValue(entity, effect.type, 1.0f / effect.val);
    }

    private void MultiplyValue(Entity entity, StatusEffectType type, float value)
    {
        switch (type)
        {
            case StatusEffectType.Armor:
            case StatusEffectType.Delicacy:
                entity.Get<DamageComp>().damageFactor *= value;
                break;

            case StatusEffectType.Weaken:
            case StatusEffectType.Rage:
                ref WeaponComp weapon = ref entity.Get<WeaponComp>();
                weapon.dpsFactor *= value;
                // for (int i = 0; i < weapon.weapons.Count; i++)
                //     weapon.weapons[i].entity?.Get<AnimComp>().timeScale = effect.val;

                ref ShieldComp shield = ref entity.Get<ShieldComp>();
                shield.dpsFactor *= value;
                // for (int i = 0; i < shield.shields.Count; i++)
                //     shield.shields[i].entity?.Get<AnimComp>().timeScale = effect.val;
                break;

            case StatusEffectType.Haste:
            case StatusEffectType.Slowness:
                entity.Get<MoveComp>().speedFactor *= value;
                break;

            case StatusEffectType.Greed:
            case StatusEffectType.Poverty:
                entity.Get<LootCollComp>().incomeFactor *= value;
                break;

            case StatusEffectType.ShortHand:
            case StatusEffectType.LongHand:
                entity.Get<LootCollComp>().radiusFactor *= value;
                break;

            default:
                break;
        }
    }
}
