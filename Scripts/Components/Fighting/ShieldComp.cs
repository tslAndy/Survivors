using Arch.Core;
using Utils;
using Weapons;

namespace Components.Fighting;

struct ShieldComp
{
    public CachedList<ShieldElem> shields;
    public float dpsFactor;
}

struct ShieldElem
{
    public Shield shield;
    public Entity? entity;

    public ShieldElem(Shield shield, Entity? entity)
    {
        this.shield = shield;
        this.entity = entity;
    }
}
