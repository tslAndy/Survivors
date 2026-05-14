using Components.Behaviour;

namespace Behaviours.Tree;

interface INode
{
    State Update(ref EntityContext ctx);
}

enum State
{
    Failure,
    Running,
    Success,
}
