using Arch.Core;
using Arch.System;
using Components.Other;

namespace Systems.Basic;

public partial class TimerSys : BaseSystem<World, float>
{
    public TimerSys(World world)
        : base(world) { }

    [Query]
    private void HandleTimer([Data] in float dt, ref TimerComp timer)
    {
        timer.time -= dt;
    }
}
