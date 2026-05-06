using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;

namespace Systems.Fighting;

partial class ShieldSys : BaseSystem<World, float>
{
    public ShieldSys(World world)
        : base(world) { }

    [Query]
    private void UpdateShield(
        [Data] in float dt,
        Entity entity,
        ref ShieldComp shield,
        ref TransformComp trs,
        ref DamageComp damage,
        ref StatusEffectComp effects
    )
    {
        if (shield.single != null)
        {
            shield.single.Update(entity, ref damage, ref effects, trs.position, dt);
            return;
        }

        for (int i = 0; i < shield.shields.Count; i++)
            shield.shields[i].Update(entity, ref damage, ref effects, trs.position, dt);
    }

    [Query]
    private void HandleDeath(in DeathComp death, ref ShieldComp shield)
    {
        if (death.isDead)
            shield.shields?.Dispose();
    }
}
