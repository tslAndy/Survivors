using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Other;
using Systems.Basic;

namespace Systems.Fighting;

partial class DeathSys : BaseSystem<World, float>
{
    private readonly CommandBuffer _commandBuffer;
    private readonly LocalTrsSys _localTrsSys;

    public DeathSys(World world, CommandBuffer commandBuffer, LocalTrsSys localTrsSys)
        : base(world)
    {
        _commandBuffer = commandBuffer;
        _localTrsSys = localTrsSys;
    }

    [Query]
    public void HandleDeath(Entity entity, in AnimComp animator, ref DeathComp death)
    {
        if (!animator.isFinished)
            return;

        death.isDead = true;
        _localTrsSys.DestroyDescendents(entity);
        _commandBuffer.Destroy(entity);
    }

    [Query]
    private void UpdateTimer([Data] in float dt, Entity entity, ref TimerDestroyComp timerDestroy)
    {
        timerDestroy.time -= dt;
        if (timerDestroy.time > 0.0f)
            return;

        _localTrsSys.DestroyDescendents(entity);
        _commandBuffer.Destroy(entity);
    }
}
