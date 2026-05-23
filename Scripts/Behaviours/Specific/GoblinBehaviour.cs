using Behaviours.Specific.Nodes;
using Behaviours.Tree;
using Components.Behaviour;
using Systems;

namespace Behaviours.Specific;

class GoblinBehaviour : BaseBehaviour
{
    private readonly INode _node;

    public GoblinBehaviour(WorldContext context)
        : base(context)
    {
        INode run = new Sequence(new DistanceLeaf(context), new MoveTowardsLeaf(context));
        INode attack = new AttackLeaf(context);

        _node = new Selector(run, attack);
    }

    public override void Update(float dt, ref EntityContext entityCtx)
    {
        _node.Update(dt, ref entityCtx);
    }
}
