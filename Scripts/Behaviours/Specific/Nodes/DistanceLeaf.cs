using System.Numerics;
using Behaviours.Tree;
using Components.Behaviour;
using Systems;

namespace Behaviours.Specific.Nodes;

public class DistanceLeaf : BaseLeaf
{
    private const float ATTACK_DIST = 4.0f;

    public DistanceLeaf(WorldContext context)
        : base(context) { }

    public override State Update(float dt, ref EntityContext ctx)
    {
        float dist = Vector2.Distance(context.playerPosition, ctx.trs.position);
        return dist > ATTACK_DIST ? State.Success : State.Failure;
    }
}
