using Arch.Core;
using Components.Basic;
using Components.Physics;

namespace Components.Behaviour;

struct BehaviourComp
{
    public IBehaviour behaviour;
    public int state;
}

ref struct EntityContext
{
    public Entity entity;
    public ref BehaviourComp behaviour;
    public ref TrsComp trs;
    public ref AnimComp animator;
    public ref RigidComp rigid;
    public ref CollComp coll;
    public ref MoveComp move;
    public ref ModComp mod;
}

interface IBehaviour
{
    void Update(ref EntityContext entityCtx);
}
