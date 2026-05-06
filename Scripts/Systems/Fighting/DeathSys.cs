using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Components.Basic;
using Components.Fighting;
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
        if (timerDestroy.time > 0.0f)
            return;

        if (entity.Has<DamageComp>())
            entity.Get<DamageComp>().hits.Add(new Hit(1_000_000, StatusEffectType.None));
        else
            _commandBuffer.Destroy(entity);
    }
}
