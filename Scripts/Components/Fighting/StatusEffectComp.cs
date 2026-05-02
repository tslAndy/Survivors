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
    Armor,
    Blessed,
    Curse,
    Freeze,
    Haste,
    Invisibility,
    Rage,
    Slowness,
    Weaken,
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
    Armor = 1 << StatusEffectType.Armor,
    Blessed = 1 << StatusEffectType.Bleed,
    Curse = 1 << StatusEffectType.Curse,
    Freeze = 1 << StatusEffectType.Freeze,
    Haste = 1 << StatusEffectType.Haste,
    Invisibility = 1 << StatusEffectType.Invisibility,
    Rage = 1 << StatusEffectType.Rage,
    Slowness = 1 << StatusEffectType.Slowness,
    Weaken = 1 << StatusEffectType.Weaken,

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
