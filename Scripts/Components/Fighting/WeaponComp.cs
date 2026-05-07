using Arch.Core;
using Utils;
using Weapons;

namespace Components.Fighting;

struct WeaponComp
{
    public CachedList<WeaponElem> weapons;
    public float dpsFactor;
}

struct WeaponElem
{
    public Weapon weapon;
    public Entity? entity;

    public WeaponElem(Weapon weapon, Entity? entity)
    {
        this.weapon = weapon;
        this.entity = entity;
    }
}
