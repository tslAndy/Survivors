using System.Numerics;
using Behaviours.Tree;
using Components.Behaviour;
using Systems;

namespace Behaviours.Specific.Nodes;

public class DistanceLeaf : BaseLeaf
{
    private readonly float _dist;

    public DistanceLeaf(WorldContext context, float dist)
        : base(context) => _dist = dist;

    public override State Update(float dt, ref EntityContext ctx)
    {
        float dist = Vector2.DistanceSquared(context.playerPosition, ctx.trs.position);
        return dist > _dist * _dist ? State.Success : State.Failure;
    }
}
