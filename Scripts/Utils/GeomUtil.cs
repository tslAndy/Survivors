using System.Numerics;

namespace Utils;

static class GeomUtil
{
    public static bool RayCircleIntersect(
        Vector2 circlePos,
        float circleRad,
        Vector2 rayStart,
        Vector2 rayEnd,
        Vector2 rayNorm
    )
    {
        Vector2 proj = circlePos - rayNorm * Vector2.Dot(rayNorm, circlePos - rayStart);
        if (Vector2.Dot(proj - rayStart, rayEnd - rayStart) < 0)
            proj = rayStart;
        else if (Vector2.Dot(proj - rayEnd, rayStart - rayEnd) < 0)
            proj = rayEnd;

        return Vector2.DistanceSquared(proj, circlePos) < circleRad * circleRad;
    }
}
