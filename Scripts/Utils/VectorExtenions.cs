using System.Numerics;

static class VectorExtensions
{
    public static Vector2 TurnDeg(this Vector2 vec, float angle)
    {
        angle = Single.DegreesToRadians(angle);
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Vector2(vec.X * cos - vec.Y * sin, vec.X * sin + vec.Y * cos);
    }

    public static Vector2 TurnRad(this Vector2 vec, float angle)
    {
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Vector2(vec.X * cos - vec.Y * sin, vec.X * sin + vec.Y * cos);
    }
}
