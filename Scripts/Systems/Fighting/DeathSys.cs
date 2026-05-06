using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Other;

namespace Systems.Fighting;

partial class DeathSys : BaseSystem<World, float>
{
    private readonly CommandBuffer _commandBuffer;

    public DeathSys(World world, CommandBuffer commandBuffer)
        : base(world)
    {
        _commandBuffer = commandBuffer;
    }

    [Query]
    public void HandleDeath(Entity entity, in AnimComp animator, ref DeathComp death)
    {
        if (!animator.isFinished)
            return;

        death.isDead = true;
        _commandBuffer.Destroy(entity);
    }

    [Query]
    private void UpdateTimer([Data] in float dt, Entity entity, ref TimerDestroyComp timerDestroy)
    {
        timerDestroy.time -= dt;
        if (timerDestroy.time < 0.0f)
            _commandBuffer.Destroy(entity);
    }
}
