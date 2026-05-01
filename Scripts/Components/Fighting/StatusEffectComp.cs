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
    Fire,
    Bleed,
    Poison,
}
