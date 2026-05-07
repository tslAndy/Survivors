using System.Numerics;
using Arch.Core;
using Utils;

namespace Components.Fighting;

struct WeaponComp
{
    public CachedList<WeaponElem> weapons;
    public float dpsFactor;
}

interface IWeapon
{
    void Update(Entity entity, Vector2 position, float dt);
}

struct WeaponElem
{
    public IWeapon weapon;
    public Entity? entity;

    public WeaponElem(IWeapon weapon, Entity? entity)
    {
        this.weapon = weapon;
        this.entity = entity;
    }
}
