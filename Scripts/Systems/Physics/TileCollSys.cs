using System.Numerics;
using Arch.Core;
using Arch.System;
using Components.Other;
using Components.Physics;
using Engine.Common;
using Utils;

namespace Systems.Physics;

partial class TileCollSys : BaseSystem<World, float>
{
    public TileCollSys(World world)
        : base(world) { }

    public override void Update(in float dt) { }

    public void GetOverlap(
        Vector2 position,
        float radius,
        int targetLayer,
        CachedList<TileColl> result
    )
    {
        OverlapCheckQuery(World, in position, in radius, targetLayer, result);
    }

    [Query]
    private void OverlapCheck(
        [Data] in Vector2 position,
        [Data] in float radius,
        [Data] int targetLayer,
        [Data] CachedList<TileColl> result,
        in TilemapComp tilemap,
        in RigidComp rigid
    )
    {
        if (targetLayer != rigid.layer)
            return;

        int xStart = (int)MathF.Floor(position.X - radius);
        int yStart = (int)MathF.Floor(position.Y - radius);
        int xEnd = (int)MathF.Ceiling(position.X + radius);
        int yEnd = (int)MathF.Ceiling(position.Y + radius);

        for (int y = yStart; y < yEnd; y++)
        {
            for (int x = xStart; x < xEnd; x++)
            {
                if (tilemap.tilemap[x, y] == null)
                    continue;

                if (TryColl(position, radius, new Vector2Int(x, y), out TileColl tileColl))
                    result.Add(tileColl);
            }
        }
    }

    private bool TryColl(Vector2 position, float radius, Vector2Int tilePos, out TileColl tileColl)
    {
        Vector2 rectPos = new Vector2(tilePos.x + 0.5f, tilePos.y + 0.5f);
        Vector2 axis = Vector2.Normalize(rectPos - position);

        float a = Vector2.Dot(axis, rectPos + new Vector2(-0.5f, -0.5f));
        float b = Vector2.Dot(axis, rectPos + new Vector2(-0.5f, 0.5f));
        float c = Vector2.Dot(axis, rectPos + new Vector2(0.5f, 0.5f));
        float d = Vector2.Dot(axis, rectPos + new Vector2(0.5f, -0.5f));

        float rectProj = a;
        Vector2 normA = new Vector2(0.0f, -1.0f);
        if (b < rectProj)
        {
            normA = new Vector2(-1.0f, 0.0f);
            rectProj = b;
        }
        if (c < rectProj)
        {
            normA = new Vector2(0.0f, 1.0f);
            rectProj = c;
        }
        if (d < rectProj)
        {
            normA = new Vector2(1.0f, 0.0f);
            rectProj = d;
        }

        float circProj = Vector2.Dot(axis, position + axis * radius);
        if (circProj < rectProj)
        {
            tileColl = default;
            return false;
        }

        Vector2 normB = new Vector2(normA.Y, -normA.X);
        Vector2 bestNorm = Vector2.Dot(axis, normA) < Vector2.Dot(axis, normB) ? normA : normB;
        tileColl = new TileColl(tilePos, bestNorm, circProj - rectProj);
        return true;
    }
}

struct TileColl
{
    public Vector2Int tilePosition;
    public Vector2 normal;
    public float depth;

    public TileColl(Vector2Int tilePosition, Vector2 normal, float depth)
    {
        this.tilePosition = tilePosition;
        this.normal = normal;
        this.depth = depth;
    }
}
