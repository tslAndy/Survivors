using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Engine.Animations;
using Engine.Common;

namespace Systems.Fighting;

partial class HealthSys : BaseSystem<World, float>
{
    private readonly CommandBuffer _commandBuffer;
    private readonly Hash DieHash = AnimAtlas.CountHash("Die");

    public HealthSys(World world, CommandBuffer commandBuffer)
        : base(world)
    {
        _commandBuffer = commandBuffer;
    }

    [Query]
    [None(typeof(DeathComp))]
    private void UpdateHealth(Entity entity, in HealthComp health, ref AnimComp animator)
    {
        if (health.currentHP > 0)
            return;

        animator.anim = animator.atlas[DieHash, (int)animator.animDir];
        _commandBuffer.Add<DeathComp>(entity);
    }
}
