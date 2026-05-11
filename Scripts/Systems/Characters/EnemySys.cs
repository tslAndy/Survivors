using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Components.Basic;
using Components.Characters;
using Components.Physics;
using Engine.Common;
using Systems.Basic;

namespace Systems.Characters;

partial class EnemySys : BaseSystem<World, float>
{
    public EnemySys(World world)
        : base(world) { }

    [Query]
    [None(typeof(DeathComp))]
    private void UpdateEnemy(
        Entity entity,
        ref EnemyComp enemy,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref MoveComp moveComp,
        ref ModComp modComp
    )
    {
        enemy.behaviour.Update(entity, ref enemy, ref trs, ref rigid, ref moveComp, ref modComp);
    }
}

abstract class EnemyBehaviour : IEnemyBehaviour
{
    protected readonly WorldContext context;

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
            context.world.GetEntities(new QueryDescription().WithAll<PlayerComp>(), span);
            _player = span[0];
            return _player.Value;
        }
    }

    protected EnemyBehaviour(WorldContext context) => this.context = context;

    public abstract void Update(
        Entity entity,
        ref EnemyComp enemy,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref MoveComp moveComp,
        ref ModComp modComp
    );
}

class GoblinBehaviour : EnemyBehaviour
{
    private readonly Hash _moveFactor;

    public GoblinBehaviour(WorldContext context, ModRegistry modRegistry)
        : base(context)
    {
        _moveFactor = modRegistry["moveFactor"];
    }

    public override void Update(
        Entity entity,
        ref EnemyComp enemy,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref MoveComp moveComp,
        ref ModComp modComp
    )
    {
        Vector2 pos = player.Get<TrsComp>().position;
        rigid.velocity =
            modComp[_moveFactor] * moveComp.maxSpeed * Vector2.Normalize(pos - trs.position);
    }
}
