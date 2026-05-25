using Arch.Core;
using Arch.System;

namespace Systems.Basic;

class LevelSys : BaseSystem<World, float>
{
    public Level? level;

    public LevelSys(World world)
        : base(world) { }

    public override void Update(in float dt) => level?.Update(dt);
}
