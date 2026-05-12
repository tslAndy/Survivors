using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Fighting;
using Engine.Common;
using Systems.Basic;

class StatusEffectHandler
{
    private const float MAX_DURATION = 5.0f;
    private const float MAX_DPS = 100;

    private readonly Hash MoveHash = ModRegistry.CountHash("moveFactor"),
        AnimTimeHash = ModRegistry.CountHash("animTimeFactor"),
        DetectRadiusHash = ModRegistry.CountHash("detectRadiusFactor"),
        AttackSpeedHash = ModRegistry.CountHash("attackSpeedFactor"),
        DamageTakeHash = ModRegistry.CountHash("damageTakeFactor"),
        DamageGiveHash = ModRegistry.CountHash("damageGiveFactor"),
        LootIncomeHash = ModRegistry.CountHash("lootIncomeFactor"),
        LootRadiusHash = ModRegistry.CountHash("lootRadiusFactor"),
        LootDropHash = ModRegistry.CountHash("lootDropFactor");

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
            case StatusEffectType.DamageIncrease:
            case StatusEffectType.AttackSpeedIncrease:
            case StatusEffectType.FarSight:
            case StatusEffectType.Haste:
            case StatusEffectType.Greed:
            case StatusEffectType.LongHand:
            case StatusEffectType.RichDrop:
                float oldVal = first.val;
                first.duration = Math.Clamp(first.duration + second.duration, 0.0f, MAX_DURATION);
                first.val = Math.Max(first.val, second.val);
                MultiplyValue(entity, first.type, first.val / oldVal);
                break;

            // effects combined by min
            case StatusEffectType.DamageDecrease:
            case StatusEffectType.AttackSpeedDescrease:
            case StatusEffectType.ShortSight:
            case StatusEffectType.Slowness:
            case StatusEffectType.Stuck:
            case StatusEffectType.Poverty:
            case StatusEffectType.ShortHand:
            case StatusEffectType.PoorDrop:
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
                entity.Get<ModComp>()[DamageTakeHash] *= value;
                break;

            case StatusEffectType.DamageDecrease:
            case StatusEffectType.DamageIncrease:
                entity.Get<ModComp>()[DamageGiveHash] *= value;
                break;

            case StatusEffectType.AttackSpeedIncrease:
            case StatusEffectType.AttackSpeedDescrease:
                entity.Get<ModComp>()[AttackSpeedHash] *= value;

                ref WeaponComp weapon = ref entity.Get<WeaponComp>();
                for (int i = 0; i < weapon.weapons.Count; i++)
                    weapon.weapons[i].entity?.Get<ModComp>()[AnimTimeHash] *= value;

                ref ShieldComp shield = ref entity.Get<ShieldComp>();
                for (int i = 0; i < shield.shields.Count; i++)
                    shield.shields[i].entity?.Get<ModComp>()[AnimTimeHash] *= value;

                break;

            case StatusEffectType.Haste:
            case StatusEffectType.Slowness:
                entity.Get<ModComp>()[MoveHash] *= value;
                break;

            case StatusEffectType.ShortSight:
            case StatusEffectType.FarSight:
                entity.Get<ModComp>()[DetectRadiusHash] *= value;
                break;

            case StatusEffectType.Greed:
            case StatusEffectType.Poverty:
                entity.Get<ModComp>()[LootIncomeHash] *= value;
                break;

            case StatusEffectType.ShortHand:
            case StatusEffectType.LongHand:
                entity.Get<ModComp>()[LootRadiusHash] *= value;
                break;

            case StatusEffectType.PoorDrop:
            case StatusEffectType.RichDrop:
                entity.Get<ModComp>()[LootDropHash] *= value;
                break;

            default:
                break;
        }
    }
}
