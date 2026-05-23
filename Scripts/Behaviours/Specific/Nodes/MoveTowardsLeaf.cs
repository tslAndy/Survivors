using System.Numerics;
using Behaviours.Tree;
using Components.Basic;
using Components.Behaviour;
using Engine.Animations;
using Engine.Common;
using Systems;

namespace Behaviours.Specific.Nodes;

class MoveTowardsLeaf : BaseLeaf
{
    private readonly Hash WalkHash = AnimAtlas.CountHash("Walk");

    public MoveTowardsLeaf(WorldContext context)
        : base(context) { }

    public override State Update(float dt, ref EntityContext ctx)
    {
        Vector2 delta = context.playerPosition - ctx.trs.position;
        ctx.rigid.velocity = Vector2.Normalize(delta) * ctx.move.maxSpeed;

        AnimDir animDir = delta.AsAnimDir();
        if (ctx.animator.groupHash != WalkHash || ctx.animator.animDir != animDir)
            ctx.animator.SetAnimByGroup(WalkHash, animDir);

        return State.Success;
    }
}
