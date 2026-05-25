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

abstract class Level : IDisposable
{
    public float time { get; protected set; }
    protected readonly World world;

    protected Level(World world) => this.world = world;

    public void Update(float dt)
    {
        time += dt;
        OnUpdate(dt);
    }

    protected abstract void OnUpdate(float dt);
    public abstract void Dispose();
}
