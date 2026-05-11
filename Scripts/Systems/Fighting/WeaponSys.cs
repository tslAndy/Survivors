using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Engine.Common;
using Systems.Basic;

namespace Systems.Fighting;

partial class WeaponSys : BaseSystem<World, float>
{
    private readonly Hash _attackSpeedFactor;

    public WeaponSys(World world, ModRegistry modRegistry)
        : base(world)
    {
        _attackSpeedFactor = modRegistry["attackSpeedFactor"];
    }

    [Query]
    [None(typeof(DeathComp))]
    private void UpdateWeapon(
        [Data] in float dt,
        Entity entity,
        in TrsComp trs,
        ref WeaponComp weapon,
        ref ModComp modComp
    )
    {
        float dts = dt * modComp[_attackSpeedFactor];
        for (int i = 0; i < weapon.weapons.Count; i++)
            weapon.weapons[i].weapon.Update(entity, ref modComp, trs.position, dts);
    }

    [Query]
    private void HandleDeath(in DeathComp death, ref WeaponComp weapon)
    {
        if (death.isDead)
            weapon.weapons?.Dispose();
    }
}
