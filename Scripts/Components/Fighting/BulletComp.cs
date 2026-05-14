using Arch.Core;
using Components.Basic;
using Components.Other;
using Components.Physics;

namespace Components.Fighting;

struct BulletComp
{
    public Entity owner;
    public Entity? extra;
    public IBulletWeapon weapon;
}

interface IBulletWeapon
{
    void UpdateBullet(
        Entity owner,
        Entity? extra,
        Entity bullet,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref CollComp coll,
        ref TimerComp timer
    );
}
