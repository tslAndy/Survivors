using System.Numerics;
using Behaviours.Tree;
using Components.Basic;
using Components.Behaviour;
using Engine.Animations;
using Engine.Common;
using Systems;
using Systems.Basic;

namespace Behaviours.Specific.Nodes;

class MoveTowardsLeaf : BaseLeaf
{
    private readonly Hash WalkHash = AnimAtlas.CountHash("Walk");
    private readonly Hash MoveHash = ModRegistry.CountHash("moveFactor");

    public MoveTowardsLeaf(WorldContext context)
        : base(context) { }

    public override State Update(float dt, ref EntityContext ctx)
    {
        Vector2 delta = context.playerPosition - ctx.trs.position;
        ctx.rigid.velocity = ctx.mod[MoveHash] * ctx.move.maxSpeed * Vector2.Normalize(delta);

        AnimDir animDir = delta.AsAnimDir();
        if (ctx.animator.groupHash != WalkHash || ctx.animator.animDir != animDir)
            ctx.animator.SetAnimByGroup(WalkHash, animDir);

        return State.Success;
    }
}
