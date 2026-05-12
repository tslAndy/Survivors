using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Components.Physics;

namespace Systems.Fighting.Specific;

partial class SpinWeaponSys : BaseSystem<World, float>
{
    public SpinWeaponSys(World world)
        : base(world) { }

    [Query]
    private void UpdateBullet(Entity bullet, ref SpinComp spin, ref TrsComp trs, ref CollComp coll)
    {
        spin.weapon.UpdateBullet(spin.owner, bullet, ref trs, ref coll);
    }
}
