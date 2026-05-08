using Arch.Core;
using Components.Basic;
using Components.Physics;

namespace Components.Fighting;

struct BulletComp
{
    public Entity owner;
    public IBulletWeapon weapon;
}

interface IBulletWeapon
{
    void UpdateBullet(
        Entity owner,
        Entity bullet,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref CollComp coll
    );
}
