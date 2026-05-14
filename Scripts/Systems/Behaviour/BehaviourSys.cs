using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Behaviour;
using Components.Fighting;
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
    [None(typeof(DeathComp))]
    private void UpdatePlayerInfo(Entity entity, ref TrsComp trs)
    {
        _context.player = entity;
        _context.playerPosition = trs.position;
    }

    [Query]
    [None(typeof(DeathComp))]
    private void UpdateBehaviour(
        [Data] in float dt,
        Entity entity,
        ref BehaviourComp behaviour,
        ref TrsComp trs,
        ref AnimComp animator,
        ref RigidComp rigid,
        ref CollComp coll,
        ref MoveComp move,
        ref ModComp mod,
        ref WeaponComp weapon
    )
    {
        EntityContext entityCtx = new EntityContext
        {
            entity = entity,
            trs = ref trs,
            animator = ref animator,
            rigid = ref rigid,
            coll = ref coll,
            move = ref move,
            mod = ref mod,
            weapon = ref weapon,
        };

        behaviour.behaviour.Update(dt, ref entityCtx);
    }
}
