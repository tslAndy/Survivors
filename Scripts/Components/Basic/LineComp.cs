using System.Numerics;
using Raylib_cs;
using Utils;

namespace Components.Basic;

struct LineComp
{
    public CachedList<Line> lines;
    public int drawOrder;
}

struct Line
{
    public Color color;
    public Vector2 start,
        end;
    public float thick;
}
