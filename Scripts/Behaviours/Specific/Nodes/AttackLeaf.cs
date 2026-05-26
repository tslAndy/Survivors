using System.Numerics;
using Behaviours.Tree;
using Components.Behaviour;
using Components.Fighting;
using Engine.Animations;
using Engine.Common;
using Systems;
using Utils;

namespace Behaviours.Specific.Nodes;

public class AttackLeaf : BaseLeaf
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
