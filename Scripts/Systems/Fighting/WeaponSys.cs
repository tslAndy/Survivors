using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;

namespace Systems.Fighting;

partial class WeaponSys : BaseSystem<World, float>
{
    public WeaponSys(World world)
        : base(world) { }

    [Query]
    [None(typeof(DeathComp))]
    private void UpdateWeapon(
        [Data] in float dt,
        Entity entity,
        in TrsComp trs,
        ref WeaponComp weapon
    )
    {
        float dts = dt * weapon.dpsFactor;
        for (int i = 0; i < weapon.weapons.Count; i++)
            weapon.weapons[i].weapon.Update(entity, trs.position, dts);
    }

    [Query]
    private void HandleDeath(in DeathComp death, ref WeaponComp weapon)
    {
        if (death.isDead)
            weapon.weapons?.Dispose();
    }
}
