using Components.Behaviour;

namespace Behaviours.Tree;

class Sequence : INode
{
    private readonly INode[] _children;

    public Sequence(params INode[] children) => this._children = children;

    public State Update(ref EntityContext ctx)
    {
        foreach (INode node in _children)
        {
            State state = node.Update(ref ctx);
            if (state != State.Success)
                return state;
        }
        return State.Success;
    }
}
