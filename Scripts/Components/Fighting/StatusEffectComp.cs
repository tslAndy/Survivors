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

    // attack
    Armor,
    Rage,
    Weaken,

    // speed
    Slowness,
    Freeze,
    Haste,

    // other
    Blessed,
    Curse,
    Invisibility,
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
    // attack
    Armor = 1 << StatusEffectType.Armor,
    Rage = 1 << StatusEffectType.Rage,
    Weaken = 1 << StatusEffectType.Weaken,

    // speed
    Slowness = 1 << StatusEffectType.Slowness,
    Freeze = 1 << StatusEffectType.Freeze,
    Haste = 1 << StatusEffectType.Haste,

    // other
    Blessed = 1 << StatusEffectType.Bleed,
    Curse = 1 << StatusEffectType.Curse,
    Invisibility = 1 << StatusEffectType.Invisibility,

    // combined
    SimpleEffects = Burn | Bleed | Poison | Shock | Regen,
    ComplexEffects =
        Armor | Blessed | Curse | Freeze | Haste | Invisibility | Rage | Slowness | Weaken,
}

static class LongStatEffTypeExtensions
{
    public static bool CheckFlag(this LongStatEffType effType, StatusEffectType flag) =>
        (((long)effType) & (1L << (int)flag)) != 0;
}
