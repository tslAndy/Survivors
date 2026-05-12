using System.Numerics;
using Arch.Core;
using Utils;

namespace Components.Basic;

struct TrsComp
{
    public Vector2 position;
    public float rotation,
        scale;

    public CachedList<Entity> descs;
}
