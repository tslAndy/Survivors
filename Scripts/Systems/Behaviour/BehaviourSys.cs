using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Behaviour;
using Components.Behaviours;
using Components.Physics;

namespace Systems.Behaviour;

partial class BehaviourSys : BaseSystem<World, float>
{
    private readonly WorldContext _context;

    public BehaviourSys(WorldContext context)
        : base(context.world)
    {
        _context = context;
    }

    [Query]
    [All(typeof(PlayerTag))]
    private void UpdatePlayerInfo(Entity entity, ref TrsComp trs)
    {
        _context.player = entity;
        _context.playerPos = trs.position;
    }

    [Query]
    private void UpdateBehaviour(
        Entity entity,
        ref BehaviourComp behaviour,
        ref TrsComp trs,
        ref AnimComp animator,
        ref RigidComp rigid,
        ref CollComp coll,
        ref MoveComp move,
        ref ModComp mod
    )
    {
        EntityContext entityCtx = new EntityContext
        {
            entity = entity,
            behaviour = ref behaviour,
            trs = ref trs,
            animator = ref animator,
            rigid = ref rigid,
            coll = ref coll,
            move = ref move,
            mod = ref mod,
        };

        behaviour.behaviour.Update(ref entityCtx);
    }
}
