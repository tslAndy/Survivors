using Arch.Core;
using Utils;
using Weapons;

namespace Components.Fighting;

struct ShieldComp
{
    public CachedList<(Shield shield, Entity? ent)> shields;
    public float dpsFactor;
}
