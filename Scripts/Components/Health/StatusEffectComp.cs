using Utils;

namespace Components.Health;

public struct StatusEffectComp
{
    public float time;

    public CachedList<StatusEffect> newEffects,
        runningEffects;
}

public struct StatusEffect
{
    public StatusEffectType type;
    public float duration;
    public float val;

    public StatusEffect(StatusEffectType type, float duration, float val)
    {
        this.type = type;
        this.duration = duration;
        this.val = val;
    }
}

public enum StatusEffectType : byte
{
    None,

    // health effects
    Burn,
    Bleed,
    Poison,
    Shock,
    Regen,

    // defense
    Armor,
    Delicacy,

    // damage
    DamageIncrease,
    DamageDecrease,

    // attack
    AttackFast,
    AttackSlow,

    // bullet
    BulletFast,
    BulletSlow,

    // radius
    ShortSight,
    FarSight,

    // speed
    Slowness,
    Haste,
    Stuck,

    // money income
    Poverty,
    Greed,

    // money collect
    ShortHand,
    LongHand,

    // money drop
    PoorDrop,
    RichDrop,

    // effects time scale
    LongNegativeEffect,
    ShortNegativeEffect,

    LongPositiveEffect,
    ShortPositiveEffect,
}

enum LongStatEffType : long
{
    // health effects
    Burn = 1L << StatusEffectType.Burn,
    Bleed = 1L << StatusEffectType.Bleed,
    Poison = 1L << StatusEffectType.Poison,
    Shock = 1L << StatusEffectType.Shock,
    Regen = 1L << StatusEffectType.Regen,

    // defense
    Armor = 1L << StatusEffectType.Armor,
    Delicacy = 1L << StatusEffectType.Delicacy,

    // damage
    DamageIncrease = 1L << StatusEffectType.DamageIncrease,
    DamageDecrease = 1L << StatusEffectType.DamageDecrease,

    // attack
    AttackFast = 1L << StatusEffectType.AttackFast,
    AttackSlow = 1L << StatusEffectType.AttackSlow,

    // bullet speed
    BulletFast = 1L << StatusEffectType.BulletFast,
    BulletSlow = 1L << StatusEffectType.BulletSlow,

    // radius
    ShortSight = 1L << StatusEffectType.ShortSight,
    FarSight = 1L << StatusEffectType.FarSight,

    // speed
    Slowness = 1L << StatusEffectType.Slowness,
    Haste = 1L << StatusEffectType.Haste,
    Stuck = 1L << StatusEffectType.Stuck,

    // money income
    Greed = 1L << StatusEffectType.Greed,
    Poverty = 1L << StatusEffectType.Poverty,

    // money collect
    ShortHand = 1L << StatusEffectType.ShortHand,
    LongHand = 1L << StatusEffectType.LongHand,

    // money drop
    PoorDrop = 1L << StatusEffectType.PoorDrop,
    RichDrop = 1L << StatusEffectType.RichDrop,

    // effects time scale
    LongNegativeEffect = 1L << StatusEffectType.LongNegativeEffect,
    ShortNegativeEffect = 1L << StatusEffectType.ShortNegativeEffect,

    LongPositiveEffect = 1L << StatusEffectType.LongPositiveEffect,
    ShortPositiveEffect = 1L << StatusEffectType.ShortPositiveEffect,

    // combined
    SimpleEffects = Burn | Bleed | Poison | Shock | Regen,

    NegativeEffects =
        Burn
        | Bleed
        | Poison
        | Shock
        | Delicacy
        | DamageDecrease
        | AttackSlow
        | BulletSlow
        | ShortSight
        | Slowness
        | Stuck
        | Poverty
        | ShortHand
        | PoorDrop,

    PositiveEffects =
        Regen
        | Armor
        | DamageIncrease
        | AttackFast
        | BulletFast
        | FarSight
        | Haste
        | Greed
        | LongHand
        | RichDrop,
}

static class LongStatEffTypeExtensions
{
    public static bool CheckFlag(this LongStatEffType effType, StatusEffectType flag) =>
        (((long)effType) & (1L << (int)flag)) != 0;
}
