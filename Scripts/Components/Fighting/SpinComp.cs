using Arch.Core;
using Components.Basic;
using Components.Physics;

namespace Components.Fighting;

struct SpinComp
{
    public ISpinWeapon weapon;
    public Entity owner;
}

interface ISpinWeapon
{
    void UpdateBullet(Entity owner, Entity bullet, ref TrsComp globalTrs, ref CollComp coll);
}
