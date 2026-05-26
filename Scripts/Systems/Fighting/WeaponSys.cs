using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Engine.Common;
using Systems.Basic;

namespace Systems.Fighting;

public partial class WeaponSys : BaseSystem<World, float>
{
    private readonly Hash AttackSpeedHash = ModRegistry.CountHash("attackSpeedFactor");

    public WeaponSys(World world)
        : base(world) { }

    [Query]
    [None(typeof(DeathComp), typeof(EnemyTag))]
    private void UpdateWeapon(
        [Data] in float dt,
        Entity entity,
        in TrsComp trs,
        ref WeaponComp weapon,
        ref ModComp modComp
    )
    {
        float dts = dt * modComp[AttackSpeedHash];
        for (int i = 0; i < weapon.weapons.Count; i++)
        {
            ref WeaponElem elem = ref weapon.weapons[i];
            elem.weapon.Update(entity, elem.entity, ref modComp, trs.position, dts);
        }
    }
}
