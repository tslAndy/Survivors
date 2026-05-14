using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Components.Health;
using Engine.Common;
using Systems.Basic;

namespace Systems.Fighting;

partial class ShieldSys : BaseSystem<World, float>
{
    private readonly Hash AttackSpeedHash = ModRegistry.CountHash("attackSpeedFactor");

    public ShieldSys(World world)
        : base(world) { }

    [Query]
    private void UpdateShield(
        [Data] in float dt,
        Entity entity,
        ref ShieldComp shield,
        ref TrsComp trs,
        ref DamageComp damage,
        ref StatusEffectComp effects,
        ref ModComp modComp
    )
    {
        float dts = dt * modComp[AttackSpeedHash];
        for (int i = 0; i < shield.shields.Count; i++)
        {
            ref ShieldElem elem = ref shield.shields[i];
            elem.shield.Update(
                entity,
                elem.entity,
                ref damage,
                ref effects,
                ref modComp,
                trs.position,
                dts
            );
        }
    }

    // WARNING: enemy shields should be disposed manually
    // because list is shared
    [Query]
    [None(typeof(EnemyTag))]
    private void HandleDeath(in DeathComp death, ref ShieldComp shield)
    {
        if (death.isDead)
            shield.shields?.Dispose();
    }
}
