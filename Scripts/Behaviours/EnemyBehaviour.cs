using Arch.Core;
using Components.Behaviours;
using Systems;

namespace Behaviours;

abstract class EnemyBehaviour : BaseBehaviour
{
    private Entity? _player;
    protected Entity player
    {
        get
        {
            if (_player != null && !context.world.IsAlive(_player.Value))
                _player = null;

            if (_player != null)
                return _player.Value;

            Span<Entity> span = stackalloc Entity[1];
            context.world.GetEntities(new QueryDescription().WithAll<PlayerTag>(), span);
            _player = span[0];
            return _player.Value;
        }
    }

    protected EnemyBehaviour(WorldContext context)
        : base(context) { }
}
