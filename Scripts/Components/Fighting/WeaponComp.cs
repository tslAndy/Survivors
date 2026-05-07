using Arch.Core;
using Utils;
using Weapons;

namespace Components.Fighting;

struct WeaponComp
{
    public CachedList<(Weapon weapon, Entity? ent)> weapons;
    public float dpsFactor;
}
