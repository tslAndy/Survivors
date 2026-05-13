using System.Numerics;
using Components.Behaviour;
using Engine.Common;
using Systems;
using Systems.Basic;

namespace Behaviours.Specific;

class GoblinBehaviour : BaseBehaviour
{
    private readonly Hash MoveFactorHash = ModRegistry.CountHash("moveFactor");

    public GoblinBehaviour(WorldContext context)
        : base(context) { }

    public override void Update(ref EntityContext entityCtx)
    {
        entityCtx.rigid.velocity =
            entityCtx.mod[MoveFactorHash]
            * entityCtx.move.maxSpeed
            * Vector2.Normalize(context.playerPos - entityCtx.trs.position);
    }
}
