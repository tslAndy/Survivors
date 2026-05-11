using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Engine.Common;
using Systems.Basic;

namespace Systems.Fighting;

partial class ShieldSys : BaseSystem<World, float>
{
    private readonly Hash _attackSpeedFactor;

    public ShieldSys(World world, ModRegistry modRegistry)
        : base(world)
    {
        _attackSpeedFactor = modRegistry["attackSpeedFactor"];
    }

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
        float dts = dt * modComp[_attackSpeedFactor];
        for (int i = 0; i < shield.shields.Count; i++)
            shield
                .shields[i]
                .shield.Update(entity, ref damage, ref effects, ref modComp, trs.position, dts);
    }

    [Query]
    private void HandleDeath(in DeathComp death, ref ShieldComp shield)
    {
        if (death.isDead)
            shield.shields?.Dispose();
    }
}
