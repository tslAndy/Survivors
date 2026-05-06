using Arch.Core;
using Utils;

namespace Components.Fighting;

struct DamageComp
{
    public CachedList<Hit> hits;
    public float damageFactor;
}

// if direct hit from entity, effect type is default (because stat effects are applied separately)
// if hit from status effect, source entity is default (because effect can be applied by two or more entitites)
struct Hit
{
    public int damage;

    // if direct damage from entity
    public Entity? source;
    public bool isCrit;

    // if damage from status effect
    public StatusEffectType effectType;

    public Hit(Entity source, int damage, bool isCrit)
    {
        this.source = source;
        this.damage = damage;
        this.isCrit = isCrit;
    }

    public Hit(int damage, StatusEffectType effectType)
    {
        this.source = null;
        this.damage = damage;
        this.effectType = effectType;
    }
}
