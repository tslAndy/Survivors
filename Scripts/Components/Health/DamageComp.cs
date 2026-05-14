using Arch.Core;
using Utils;

namespace Components.Health;

struct DamageComp
{
    public CachedList<Hit> hits;
}

// if direct hit from entity, effect type is default (because stat effects are applied separately)
// if hit from status effect, source entity is default (because effect can be applied by two or more entitites)
struct Hit
{
    public int damage;

    // if direct damage from entity
    public Entity? source;

    // if damage from status effect
    public StatusEffectType effectType;

    public Hit(Entity source, int damage)
    {
        this.source = source;
        this.damage = damage;
        this.effectType = default;
    }

    public Hit(int damage, StatusEffectType effectType)
    {
        this.source = default;
        this.damage = damage;
        this.effectType = effectType;
    }
}
