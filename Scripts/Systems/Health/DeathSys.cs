using Arch.Buffer;
using Arch.Bus;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Components.Basic;
using Components.Other;
using Events;

namespace Systems.Health;

partial class DeathSys : BaseSystem<World, float>
{
    private readonly CommandBuffer _commandBuffer;

    public DeathSys(World world, CommandBuffer commandBuffer)
        : base(world)
    {
        _commandBuffer = commandBuffer;
    }

    public override void Update(in float dt)
    {
        // WARNING: order of methods should not be changed

        HandleDeathQuery(World);
        HandleTimerQuery(World, dt);
        HandleAnimQuery(World);
    }

    [Query]
    private void HandleDeath(Entity entity, in DeathComp death)
    {
        if (death.isDead)
            Destroy(entity);
    }

    [Query]
    private void HandleTimer([Data] in float dt, Entity entity, in TimerComp timer)
    {
        if (timer.time > 0.0f)
            return;

        _commandBuffer.Add<DeathComp>(entity, new DeathComp { isDead = true });
        _commandBuffer.Remove<TimerComp>(entity);
    }

    [Query]
    private void HandleAnim(Entity entity, in AnimComp animator, ref DeathComp death)
    {
        if (animator.isFinished)
            death.isDead = true;
    }

    [Query]
    private void HandleDispose(in DispComp disp) => DisposeComp(in disp);

    // WARNING: add to dispose lines, weapons, shields, new/old effects, hits, trs descs
    private void Destroy(Entity entity)
    {
        DeathEvent deathEvent = new DeathEvent { entity = entity };
        EventBus.Send(ref deathEvent);

        _commandBuffer.Destroy(entity);

        ref TrsComp trs = ref entity.Get<TrsComp>();
        if (trs.descs != null)
            for (int i = 0; i < trs.descs.Count; i++)
                Destroy(trs.descs[i]);

        if (entity.Has<DispComp>())
        {
            ref DispComp disp = ref entity.Get<DispComp>();
            DisposeComp(in disp);
        }
    }

    private void DisposeComp(in DispComp disp)
    {
        for (int i = 0; i < 8; i++)
            if (disp[i] != null)
            {
                Console.WriteLine(disp[i].GetType());
                disp[i]!.Dispose();
            }
            else
                break;
    }
}
