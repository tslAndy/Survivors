using System.Numerics;

namespace Engine.Common;

struct AABB
{
    public Vector2 min,
        max;

    private AABB(Vector2 min, Vector2 max)
    {
        this.min = min;
        this.max = max;
    }

    public static AABB ByBounds(Vector2 min, Vector2 max) => new AABB(min, max);

    public static AABB BySize(Vector2 center, Vector2 size) =>
        new AABB(center - 0.5f * size, center + 0.5f * size);

    public static bool CheckOverlap(in AABB a, in AABB b)
    {
        return !(Vector2.LessThanAny(a.max, b.min) || Vector2.LessThanAny(b.max, a.min));
    }
}
