using Components.Behaviour;

namespace Behaviours.Tree;

public class Sequence : INode
{
    private readonly INode[] _children;

    public Sequence(params INode[] children) => this._children = children;

    public State Update(float dt, ref EntityContext ctx)
    {
        foreach (INode node in _children)
        {
            State state = node.Update(dt, ref ctx);
            if (state != State.Success)
                return state;
        }
        return State.Success;
    }
}
