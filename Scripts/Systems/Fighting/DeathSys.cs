using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Components.Other;
using Utils;

namespace Systems.Fighting;

partial class DeathSys : BaseSystem<World, float>
{
    private readonly CommandBuffer _commBuffer;

    public DeathSys(World world)
        : base(world)
    {
        _commBuffer = ServiceLocator.Get<CommandBuffer>();
    }

    [Query]
    [All(typeof(DeathComp))]
    // ADD HERE OTHER DEPS THAT NEED TO BE FREED
    public void HandleDeath(
        Entity entity,
        ref AnimComp animator,
        ref DamageComp damage,
        ref StatusEffectComp effects
    )
    {
        if (!animator.isFinished)
            return;

        damage.hits.Dispose();
        effects.newEffects.Dispose();
        effects.runningEffects.Dispose();

        // TODO: remove weapons list

        // handle other individually allocated objects for entity

        _commBuffer.Destroy(entity);
    }

    // TODO: query for weapons and shields

    [Query]
    private void UpdateTimer([Data] in float dt, Entity entity, ref TimerDestroyComp timerDestroy)
    {
        timerDestroy.time -= dt;
        if (timerDestroy.time < 0.001f)
            _commBuffer.Destroy(entity);
    }
}
