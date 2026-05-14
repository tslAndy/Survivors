using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Components.Other;
using Systems.Basic;

namespace Systems.Fighting;

partial class TimerSys : BaseSystem<World, float>
{
    private LocalTrsSys _localTrsSys;
    private CommandBuffer _commandBuffer;

    public TimerSys(World world, LocalTrsSys localTrsSys, CommandBuffer commandBuffer)
        : base(world)
    {
        _localTrsSys = localTrsSys;
        _commandBuffer = commandBuffer;
    }

    [Query]
    private void UpdateTimer([Data] in float dt, Entity entity, ref TimerComp timerDestroy)
    {
        timerDestroy.time -= dt;
        if (timerDestroy.time > 0.0f)
            return;

        _localTrsSys.DestroyDescendents(entity);
        _commandBuffer.Destroy(entity);
    }
}
