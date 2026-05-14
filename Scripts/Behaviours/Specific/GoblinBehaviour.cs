using System.Numerics;
using Behaviours.Tree;
using Components.Basic;
using Components.Behaviour;
using Components.Fighting;
using Engine.Animations;
using Engine.Common;
using Systems;
using Utils;

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

    public override void Update(float dt, ref EntityContext entityCtx)
    {
        node.Update(dt, ref entityCtx);
    }
}

class DistanceLeaf : BaseLeaf
{
    private const float ATTACK_DIST = 3.0f;

    public DistanceLeaf(WorldContext context)
        : base(context) { }

    public override State Update(float dt, ref EntityContext ctx)
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

class AttackLeaf : BaseLeaf
{
    private readonly Hash AttackHash = AnimAtlas.CountHash("Attack");

    public AttackLeaf(WorldContext context)
        : base(context) { }

    public override State Update(float dt, ref EntityContext ctx)
    {
        ctx.rigid.velocity = Vector2.Zero;

        if (ctx.animator.groupHash != AttackHash)
            ctx.animator.SetAnimByGroup(AttackHash);

        if (!ctx.animator.isFinished)
            return State.Success;

        CachedList<WeaponElem> weapons = ctx.weapon.weapons;
        for (int i = 0; i < weapons.Count; i++)
        {
            ref WeaponElem elem = ref weapons[i];
            elem.weapon.Update(ctx.entity, elem.entity, ref ctx.mod, ctx.trs.position, dt);
        }

        ctx.animator.SetAnimByGroup(AttackHash);
        return State.Success;
    }
}
