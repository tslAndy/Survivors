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
        in TransformComp trs,
        ref WeaponComp weapon
    )
    {
        for (int i = 0; i < weapon.weapons.Count; i++)
            weapon.weapons[i].Update(entity, trs.position, dt);
    }

    [Query]
    private void HandleDeath(in DeathComp death, ref WeaponComp weapon)
    {
        if (!death.isDead)
            return;

        weapon.weapons.Dispose();
    }
}
