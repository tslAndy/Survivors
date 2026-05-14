using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Components.Basic;

namespace Systems.Basic;

partial class LocalTrsSys : BaseSystem<World, float>
{
    private readonly CommandBuffer _buffer;

    public LocalTrsSys(World world, CommandBuffer buffer)
        : base(world)
    {
        _buffer = buffer;
    }

    [Query]
    [None(typeof(DeathComp), typeof(LocalTrsComp))]
    private void Handle(in TrsComp trs)
    {
        if (trs.descs == null)
            return;

        for (int i = 0; i < trs.descs.Count; i++)
        {
            Entity desc = trs.descs[i];
            ref LocalTrsComp descLocalTrs = ref desc.Get<LocalTrsComp>();
            ref TrsComp descGlobalTrs = ref desc.Get<TrsComp>();

            descGlobalTrs.position = trs.position + descLocalTrs.position.TurnDeg(trs.rotation);
            descGlobalTrs.rotation = trs.rotation + descLocalTrs.rotation;
            descGlobalTrs.scale = trs.scale * descLocalTrs.scale;

            Handle(in descGlobalTrs);
        }
    }
}
