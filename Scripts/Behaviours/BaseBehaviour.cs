using Components.Behaviour;
using Systems;

namespace Behaviours;

abstract class BaseBehaviour : IBehaviour
{
    protected readonly WorldContext context;

    protected BaseBehaviour(WorldContext context) => this.context = context;

    public abstract void Update(ref EntityContext entityCtx);
}
