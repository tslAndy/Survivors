using Components.Behaviour;

namespace Behaviours.Tree;

class Selector : INode
{
    private readonly INode[] _children;

    public Selector(params INode[] children) => this._children = children;

    public State Update(ref EntityContext ctx)
    {
        foreach (INode node in _children)
        {
            State state = node.Update(ref ctx);
            if (state != State.Failure)
                return state;
        }
        return State.Failure;
    }
}
