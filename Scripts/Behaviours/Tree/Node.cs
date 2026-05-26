using Components.Behaviour;

namespace Behaviours.Tree;

public interface INode
{
    State Update(float dt, ref EntityContext ctx);
}

public enum State
{
    Failure,
    Running,
    Success,
}
