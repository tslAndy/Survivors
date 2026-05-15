using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Fighting;
using Components.Health;
using Engine.Common;
using Systems.Basic;

namespace Systems.Health;

class StatusEffectHandler
{
    private const float MAX_DURATION = 5.0f;
    private const float MAX_DPS = 100;

    private readonly Hash MoveHash = ModRegistry.CountHash("moveFactor"),
        AnimTimeHash = ModRegistry.CountHash("animTimeFactor"),
        DetectRadiusHash = ModRegistry.CountHash("detectRadiusFactor"),
        AttackSpeedHash = ModRegistry.CountHash("attackSpeedFactor"),
        BulletSpeedHash = ModRegistry.CountHash("bulletSpeedFactor"),
        DamageTakeHash = ModRegistry.CountHash("damageTakeFactor"),
        DamageGiveHash = ModRegistry.CountHash("damageGiveFactor"),
        LootIncomeHash = ModRegistry.CountHash("lootIncomeFactor"),
        LootRadiusHash = ModRegistry.CountHash("lootRadiusFactor"),
        LootDropHash = ModRegistry.CountHash("lootDropFactor");

    // TODO: combine withh LONG_EFFECT enum
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
            case StatusEffectType.AttackFast:
            case StatusEffectType.BulletFast:
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
            case StatusEffectType.AttackSlow:
            case StatusEffectType.BulletSlow:
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
        ref ModComp mod = ref entity.Get<ModComp>();

        switch (type)
        {
            case StatusEffectType.Armor:
            case StatusEffectType.Delicacy:
                mod[DamageTakeHash] *= value;
                break;

            case StatusEffectType.DamageDecrease:
            case StatusEffectType.DamageIncrease:
                mod[DamageGiveHash] *= value;
                break;

            case StatusEffectType.AttackFast:
            case StatusEffectType.AttackSlow:
                mod[AttackSpeedHash] *= value;

                ref WeaponComp weapon = ref entity.Get<WeaponComp>();
                for (int i = 0; i < weapon.weapons.Count; i++)
                {
                    Entity? extra = weapon.weapons[i].entity;
                    if (extra != null && extra.Value.Has<ModComp>())
                        extra.Value.Get<ModComp>()[AnimTimeHash] *= value;
                }

                ref ShieldComp shield = ref entity.Get<ShieldComp>();
                for (int i = 0; i < shield.shields.Count; i++)
                {
                    Entity? extra = shield.shields[i].entity;
                    if (extra != null && extra.Value.Has<ModComp>())
                        extra.Value.Get<ModComp>()[AnimTimeHash] *= value;
                }

                break;

            case StatusEffectType.BulletSlow:
            case StatusEffectType.BulletFast:
                mod[BulletSpeedHash] *= value;
                break;

            case StatusEffectType.Haste:
            case StatusEffectType.Slowness:
                mod[MoveHash] *= value;
                break;

            case StatusEffectType.ShortSight:
            case StatusEffectType.FarSight:
                mod[DetectRadiusHash] *= value;
                break;

            case StatusEffectType.Greed:
            case StatusEffectType.Poverty:
                mod[LootIncomeHash] *= value;
                break;

            case StatusEffectType.ShortHand:
            case StatusEffectType.LongHand:
                mod[LootRadiusHash] *= value;
                break;

            case StatusEffectType.PoorDrop:
            case StatusEffectType.RichDrop:
                mod[LootDropHash] *= value;
                break;

            default:
                break;
        }
    }
}
