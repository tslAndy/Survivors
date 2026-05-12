using System.Numerics;
using Arch.Core;
using Components.Basic;
using Utils;

namespace Components.Fighting;

struct WeaponComp
{
    public CachedList<WeaponElem> weapons;
}

interface IWeapon
{
    void Update(Entity entity, Entity? extra, ref ModComp modComp, Vector2 position, float dt);
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
