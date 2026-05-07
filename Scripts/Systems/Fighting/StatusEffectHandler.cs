using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Characters;
using Components.Fighting;
using Components.Loot;

class StatusEffectHandler
{
    private const float MAX_DURATION = 5.0f;
    private const float MAX_DPS = 100;

    public void CombineEffects(ref StatusEffect first, ref StatusEffect second)
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
                first.duration = Math.Clamp(first.duration + second.duration, 0.0f, MAX_DURATION);
                first.val = Math.Max(first.val, second.val);
                break;

            // effects combined by min
            case StatusEffectType.Weaken:
            case StatusEffectType.Slowness:
            case StatusEffectType.Stuck:
            case StatusEffectType.Poverty:
            case StatusEffectType.ShortHand:
                first.duration = Math.Clamp(first.duration + second.duration, 0.0f, MAX_DURATION);
                first.val = Math.Min(first.val, second.val);
                break;

            case StatusEffectType.Curse:
                first.duration = Math.Min(first.duration, second.duration);
                break;

            default:
                break;
        }
    }

    public void OnEffectAdd(Entity entity, ref StatusEffect effect)
    {
        switch (effect.type)
        {
            case StatusEffectType.Armor:
            case StatusEffectType.Delicacy:
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
                entity.Get<MoveComp>().speedFactor = effect.val;
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
            case StatusEffectType.Delicacy:
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
                entity.Get<MoveComp>().speedFactor = 1.0f;
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
