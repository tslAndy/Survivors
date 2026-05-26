using System.Numerics;
using Behaviours.Tree;
using Components.Behaviour;
using Components.Fighting;
using Engine.Animations;
using Engine.Common;
using Systems;
using Systems.Basic;
using Utils;

namespace Behaviours.Specific.Nodes;

public class AttackLeaf : BaseLeaf
{
    private readonly Hash AttackHash = AnimAtlas.CountHash("Attack");
    private readonly Hash AttackFactorHash = ModRegistry.CountHash("attackSpeedFactor");
    private readonly Hash AnimFactorHash = ModRegistry.CountHash("animTimeFactor");

    public AttackLeaf(WorldContext context)
        : base(context) { }

    public override State Update(float dt, ref EntityContext ctx)
    {
        ctx.rigid.velocity = Vector2.Zero;

        if (ctx.animator.groupHash != AttackHash)
        {
            ctx.animator.SetAnimByGroup(AttackHash);
            ctx.mod[AnimFactorHash] = ctx.mod[AttackFactorHash];
        }

        if (!ctx.animator.isFinished)
            return State.Success;

        float dts = dt * ctx.mod[AttackFactorHash];
        CachedList<WeaponElem> weapons = ctx.weapon.weapons;
        for (int i = 0; i < weapons.Count; i++)
        {
            ref WeaponElem elem = ref weapons[i];
            elem.weapon.Update(ctx.entity, elem.entity, ref ctx.mod, ctx.trs.position, dts);
        }

        ctx.animator.SetAnimByGroup(AttackHash);
        return State.Success;
    }
}
