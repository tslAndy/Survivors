using System.Numerics;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Behaviour;
using Engine.Common;
using Systems;
using Systems.Basic;

namespace Behaviours.Specific;

class GoblinBehaviour : EnemyBehaviour
{
    private readonly Hash MoveFactorHash = ModRegistry.CountHash("moveFactor");

    public GoblinBehaviour(WorldContext context)
        : base(context) { }

    public override void Update(ref EntityContext entityCtx)
    {
        Vector2 pos = player.Get<TrsComp>().position;
        entityCtx.rigid.velocity =
            entityCtx.mod[MoveFactorHash]
            * entityCtx.move.maxSpeed
            * Vector2.Normalize(pos - entityCtx.trs.position);
    }
}
