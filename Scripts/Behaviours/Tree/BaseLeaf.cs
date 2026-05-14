using Components.Behaviour;
using Systems;

namespace Behaviours.Tree;

abstract class BaseLeaf : INode
{
    protected readonly WorldContext context;

    protected BaseLeaf(WorldContext context) => this.context = context;

    public abstract State Update(ref EntityContext ctx);
}
