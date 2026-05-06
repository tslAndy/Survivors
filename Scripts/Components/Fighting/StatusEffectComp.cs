using Utils;

namespace Components.Fighting;

struct StatusEffectComp
{
    public float time;

    public CachedList<StatusEffect> newEffects,
        runningEffects;
}

struct StatusEffect
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

enum StatusEffectType : byte
{
    None,

    // health effects
    Burn,
    Bleed,
    Poison,
    Shock,
    Regen,

    // special effects

    // defense
    Armor,
    Sensitivity, // extra damage from hit

    // attack
    Rage,
    Weaken,

    // speed
    Slowness,
    Haste,
    Stuck,

    // other
    Curse,

    // money
    Poverty,
    Greed,

    ShortHand,
    LongHand,
}

enum LongStatEffType : long
{
    // health effects
    Burn = 1 << StatusEffectType.Burn,
    Bleed = 1 << StatusEffectType.Bleed,
    Poison = 1 << StatusEffectType.Poison,
    Shock = 1 << StatusEffectType.Shock,
    Regen = 1 << StatusEffectType.Regen,

    // status effects
    // defend
    Armor = 1 << StatusEffectType.Armor,
    Sensitivity = 1 << StatusEffectType.Sensitivity,

    // attack
    Rage = 1 << StatusEffectType.Rage,
    Weaken = 1 << StatusEffectType.Weaken,

    // speed
    Slowness = 1 << StatusEffectType.Slowness,
    Haste = 1 << StatusEffectType.Haste,
    Stuck = 1 << StatusEffectType.Stuck,

    // other
    Curse = 1 << StatusEffectType.Curse,

    // money
    Greed = 1 << StatusEffectType.Greed,
    Poverty = 1 << StatusEffectType.Poverty,

    ShortHand = 1 << StatusEffectType.ShortHand,
    LongHand = 1 << StatusEffectType.LongHand,

    // combined
    SimpleEffects = Burn | Bleed | Poison | Shock | Regen,
}

static class LongStatEffTypeExtensions
{
    public static bool CheckFlag(this LongStatEffType effType, StatusEffectType flag) =>
        (((long)effType) & (1L << (int)flag)) != 0;
}
