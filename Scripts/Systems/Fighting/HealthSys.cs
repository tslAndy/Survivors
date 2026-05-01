using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Engine.Animations;
using Engine.Common;
using Utils;

namespace Systems.Fighting;

partial class HealthSys : BaseSystem<World, float>
{
    private readonly CommandBuffer _commBuffer;
    private readonly Hash DieHash = AnimAtlas.CountHash("Die");

    public HealthSys(World world)
        : base(world)
    {
        _commBuffer = ServiceLocator.Get<CommandBuffer>();
    }

    [Query]
    [None(typeof(DeathComp))]
    private void UpdateHealth(Entity entity, ref HealthComp health, ref AnimComp animator)
    {
        if (health.currentHP > 0)
            return;

        animator.anim = animator.atlas[DieHash, (int)animator.animDir];
        _commBuffer.Add<DeathComp>(entity);
    }
}
