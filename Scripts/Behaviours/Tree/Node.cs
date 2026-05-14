using Components.Behaviour;

namespace Behaviours.Tree;

interface INode
{
    State Update(float dt, ref EntityContext ctx);
}

enum State
{
    Failure,
    Running,
    Success,
}
