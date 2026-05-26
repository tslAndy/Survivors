using System.Numerics;
using Raylib_cs;
using Utils;

namespace Components.Basic;

public struct LineComp
{
    public CachedList<Line> lines;
    public int drawOrder;
}

public struct Line
{
    public Color color;
    public Vector2 start,
        end;
    public float thick;
}
