using System.Numerics;
using Behaviours.Tree;
using Components.Basic;
using Components.Behaviour;
using Engine.Animations;
using Engine.Common;
using Systems;
using Systems.Basic;

namespace Behaviours.Specific;

class GoblinBehaviour : BaseBehaviour
{
    private INode node;

    public GoblinBehaviour(WorldContext context)
        : base(context)
    {
        INode run = new Sequence(new DistanceLeaf(context), new MoveTowardsLeaf(context));
        INode attack = new AttackLeaf(context);

        node = new Selector(run, attack);
    }

    public override void Update(ref EntityContext entityCtx)
    {
        node.Update(ref entityCtx);
    }
}

class DistanceLeaf : BaseLeaf
{
    private const float ATTACK_DIST = 1.0f;

    public DistanceLeaf(WorldContext context)
        : base(context) { }

    public override State Update(ref EntityContext ctx)
    {
        float dist = Vector2.Distance(context.playerPosition, ctx.trs.position);
        return dist > ATTACK_DIST ? State.Success : State.Failure;
    }
}

class MoveTowardsLeaf : BaseLeaf
{
    private readonly Hash WalkHash = AnimAtlas.CountHash("Walk");

    public MoveTowardsLeaf(WorldContext context)
        : base(context) { }

    public override State Update(ref EntityContext ctx)
    {
        Vector2 delta = context.playerPosition - ctx.trs.position;
        ctx.rigid.velocity = Vector2.Normalize(delta) * ctx.move.maxSpeed;

        AnimDir animDir = delta.AsAnimDir();
        if (ctx.animator.groupHash != WalkHash || ctx.animator.animDir != animDir)
            ctx.animator.SetAnimByGroup(WalkHash, animDir);

        return State.Success;
    }
}

class AttackLeaf : BaseLeaf
{
    private readonly Hash AttackHash = AnimAtlas.CountHash("Attack");

    public AttackLeaf(WorldContext context)
        : base(context) { }

    public override State Update(ref EntityContext ctx)
    {
        ctx.rigid.velocity = Vector2.Zero;

        if (ctx.animator.groupHash != AttackHash || ctx.animator.isFinished)
            ctx.animator.SetAnimByGroup(AttackHash);

        return State.Success;
    }
}
