using System.Numerics;

namespace Engine.Common;

public struct AABB
{
    public Vector2 min,
        max;

    private AABB(Vector2 min, Vector2 max)
    {
        this.min = min;
        this.max = max;
    }

    public static AABB ByBounds(Vector2 a, Vector2 b) =>
        new AABB(Vector2.Min(a, b), Vector2.Max(a, b));

    public static AABB BySize(Vector2 center, Vector2 size) =>
        new AABB(center - 0.5f * size, center + 0.5f * size);

    public static bool CheckOverlap(in AABB a, in AABB b)
    {
        return !(Vector2.LessThanAny(a.max, b.min) || Vector2.LessThanAny(b.max, a.min));
    }
}
