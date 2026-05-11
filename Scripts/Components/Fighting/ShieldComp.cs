using System.Numerics;
using Arch.Core;
using Components.Basic;
using Utils;

namespace Components.Fighting;

struct ShieldComp
{
    public CachedList<ShieldElem> shields;
}

interface IShield
{
    void Update(
        Entity entity,
        ref DamageComp damage,
        ref StatusEffectComp effects,
        ref ModComp modComp,
        Vector2 position,
        float dt
    );
}

struct ShieldElem
{
    public IShield shield;
    public Entity? entity;

    public ShieldElem(IShield shield, Entity? entity)
    {
        this.shield = shield;
        this.entity = entity;
    }
}
