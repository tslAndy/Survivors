using Arch.Core;
using Components.Basic;
using Components.Physics;

namespace Components.Characters;

struct EnemyComp
{
    public IEnemyBehaviour behaviour;
    public int state;
}

interface IEnemyBehaviour
{
    void Update(
        Entity entity,
        ref EnemyComp enemy,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref MoveComp moveComp,
        ref ModComp modComp
    );
}
