using Arch.Bus;
using Arch.Core;
using Arch.System;
using Events;

namespace Systems.Basic;

class LevelSys : BaseSystem<World, float>
{
    public Level? level;

    private float _countdown = 1.0f;

    public LevelSys(World world)
        : base(world) { }

    public override void Update(in float dt)
    {
        if (level == null)
            return;

        level.Update(dt);

        _countdown -= dt;
        if (_countdown < 0.0f)
        {
            _countdown += 1.0f;
            SecPassEvent @event = new SecPassEvent { levelName = level.name };
            EventBus.Send(ref @event);
        }
    }
}
