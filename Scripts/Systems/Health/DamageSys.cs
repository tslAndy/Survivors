using Arch.Buffer;
using Arch.Bus;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Health;
using Engine.Common;
using Events;
using Systems.Basic;

namespace Systems.Health;

partial class DamageSys : BaseSystem<World, float>
{
    private readonly CommandBuffer _commandBuffer;
    private readonly Hash DamageTakeHash = ModRegistry.CountHash("damageTakeFactor");

    public DamageSys(World world, CommandBuffer commandBuffer)
        : base(world)
    {
        _commandBuffer = commandBuffer;
    }

    [Query]
    [None(typeof(DeathComp))]
    private void UpdateDamage(
        Entity entity,
        in DamageComp damage,
        in TrsComp trs,
        ref HealthComp health,
        ref ModComp modComp
    )
    {
        float total = 0.0f;
        for (int i = 0; i < damage.hits.Count; i++)
        {
            ref Hit hit = ref damage.hits[i];
            total += hit.damage > 0 ? hit.damage * modComp[DamageTakeHash] : hit.damage;
            // if hit.damage > 0 it's damage, and should be scaled
            // else it's a regeneration, should not be scaled
        }
        damage.hits.Reset();

        int floored = (int)MathF.Floor(total);
        if (floored == 0)
            return;

        health.currentHP = Math.Clamp(health.currentHP - floored, 0, health.maxHP);

        DamageEvent damageEvent = new DamageEvent
        {
            target = entity,
            damage = floored,
            position = trs.position,
        };
        EventBus.Send(ref damageEvent);
    }
}
