using Arch.Core;
using Components.Basic;
using Components.Fighting;
using Components.Physics;

namespace Components.Behaviour;

public struct BehaviourComp
{
    public IBehaviour behaviour;
}

public ref struct EntityContext
{
    public Entity entity;
    public ref TrsComp trs;
    public ref AnimComp animator;
    public ref RigidComp rigid;
    public ref CollComp coll;
    public ref MoveComp move;
    public ref ModComp mod;
    public ref WeaponComp weapon;
}

public interface IBehaviour
{
    void Update(float dt, ref EntityContext entityCtx);
}
