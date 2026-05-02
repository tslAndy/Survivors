using Arch.Core;
using Arch.System;

namespace Systems.Characters;

partial class EnemyMoveSys : BaseSystem<World, float>
{
    private readonly int _playerLayer;

    public EnemyMoveSys(World world, int playerLayer)
        : base(world)
    {
        _playerLayer = playerLayer;
    }
}
