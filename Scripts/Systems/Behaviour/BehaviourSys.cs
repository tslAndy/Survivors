using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Behaviour;
using Components.Physics;

namespace Systems.Behaviour;

partial class BehaviourSys : BaseSystem<World, float>
{
    public BehaviourSys(World world)
        : base(world) { }

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
