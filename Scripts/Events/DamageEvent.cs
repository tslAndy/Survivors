using System.Numerics;
using Arch.Core;

namespace Events;

public struct DamageEvent
{
    public Entity target;
    public int damage;
    public Vector2 position;
}
