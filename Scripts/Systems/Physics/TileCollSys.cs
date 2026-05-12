using System.Numerics;
using Arch.Core;
using Arch.System;
using Components.Other;
using Components.Physics;
using Engine.Common;
using Engine.Tilemaps;
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
        ColliderOverlapQuery(World, in position, in radius, in targetLayer, result);
    }

    public void GetOverlap(
        Vector2 rayStart,
        Vector2 rayEnd,
        int targetLayer,
        CachedList<TileColl> result
    )
    {
        RayOverlapQuery(World, in rayStart, in rayEnd, in targetLayer, in result);
    }

    [Query]
    private void RayOverlap(
        [Data] in Vector2 rayStart,
        [Data] in Vector2 rayEnd,
        [Data] in int targetLayer,
        [Data] in CachedList<TileColl> result,
        in TilemapComp tilemap,
        in RigidComp rigid
    )
    {
        if (rigid.layer != targetLayer)
            return;

        Vector2 delta = Vector2.Normalize(rayEnd - rayStart);
        Vector2 invDelta = new Vector2(
            MathF.Abs(delta.X) > 0.0001f ? (1.0f / delta.X) : 1e10f,
            MathF.Abs(delta.Y) > 0.0001f ? (1.0f / delta.Y) : 1e10f
        );

        int sx = Math.Sign(delta.X);
        int sy = Math.Sign(delta.Y);

        Vector2 pos = rayStart;

        int cx = sx > 0 ? (int)MathF.Floor(pos.X) : (int)MathF.Ceiling(pos.X);
        int cy = sy > 0 ? (int)MathF.Floor(pos.Y) : (int)MathF.Ceiling(pos.Y);

        Vector2 min = Vector2.Min(rayStart, rayEnd);
        Vector2 max = Vector2.Max(rayStart, rayEnd);

        TileChunk? chunk = null;
        int chunkX = int.MinValue,
            chunkY = int.MinValue;

        int moveX = 0,
            moveY = 0;

        for (int i = 0; i < 10_000; i++)
        {
            if (Vector2.LessThanAny(pos, min) || Vector2.GreaterThanAny(pos, max))
                break;

            int x = (int)MathF.Floor(pos.X);
            int y = (int)MathF.Floor(pos.Y);

            int tx = x / TileChunk.SIZE;
            int ty = y / TileChunk.SIZE;

            if (chunk == null || chunkX != tx || chunkY != ty)
            {
                chunkX = tx;
                chunkY = ty;
                chunk = tilemap.tilemap.GetChunk(chunkX, chunkY);
            }

            if (chunk == null)
            {
                int nextX = x - x % TileChunk.SIZE;
                int nextY = y - y % TileChunk.SIZE;

                if (Math.Sign(pos.X) == sx)
                    nextX += sx * TileChunk.SIZE;
                if (Math.Sign(pos.Y) == sy)
                    nextY += sy * TileChunk.SIZE;

                chunkX = nextX / TileChunk.SIZE;
                chunkY = nextY / TileChunk.SIZE;

                Vector2 n = new Vector2(nextX - pos.X, nextY - pos.Y) * invDelta;
                n.X += 0.0001f;
                n.Y += 0.0001f;

                if (n.X > 0.0f && (n.X < n.Y || n.Y < 0.0f))
                {
                    pos += n.X * delta;

                    moveX = sx;
                    moveY = 0;
                }
                else if (n.Y > 0.0f && (n.Y < n.X || n.X < 0.0f))
                {
                    pos += n.Y * delta;

                    moveX = 0;
                    moveY = sy;
                }
                else
                {
                    break;
                }

                cx = sx > 0 ? (int)MathF.Floor(pos.X) : (int)MathF.Ceiling(pos.X);
                cy = sy > 0 ? (int)MathF.Floor(pos.Y) : (int)MathF.Ceiling(pos.Y);

                continue;
            }
            if (chunk[x % TileChunk.SIZE, y % TileChunk.SIZE] != null)
            {
                Vector2 norm = new Vector2(-moveX, -moveY);
                result.Add(new TileColl(new Vector2Int(x, y), pos, norm, 0.0f));
                break;
            }

            Vector2 t = new Vector2(cx + sx - pos.X, cy + sy - pos.Y) * invDelta;
            t.X += 0.0001f;
            t.Y += 0.0001f;

            if (t.X > 0.0f && (t.X < t.Y || t.Y < 0.0f))
            {
                pos += t.X * delta;
                cx += sx;

                moveX = sx;
                moveY = 0;
            }
            else if (t.Y > 0.0f && (t.Y < t.X || t.X < 0.0f))
            {
                pos += t.Y * delta;
                cy += sy;

                moveX = 0;
                moveY = sy;
            }
            else
            {
                break;
            }
        }
    }

    [Query]
    private void ColliderOverlap(
        [Data] in Vector2 position,
        [Data] in float radius,
        [Data] in int targetLayer,
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

        Vector2 pa = rectPos + new Vector2(-0.5f, -0.5f);
        Vector2 pb = rectPos + new Vector2(-0.5f, 0.5f);
        Vector2 pc = rectPos + new Vector2(0.5f, 0.5f);
        Vector2 pd = rectPos + new Vector2(0.5f, -0.5f);

        float a = Vector2.Dot(axis, pa);
        float b = Vector2.Dot(axis, pb);
        float c = Vector2.Dot(axis, pc);
        float d = Vector2.Dot(axis, pd);

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

        circProj = Vector2.Dot(bestNorm, position - bestNorm * radius);

        rectProj = MathF.Max(
            MathF.Max(Vector2.Dot(pa, bestNorm), Vector2.Dot(pb, bestNorm)),
            MathF.Max(Vector2.Dot(pc, bestNorm), Vector2.Dot(pd, bestNorm))
        );

        float depth = rectProj - circProj;
        Vector2 point = position - bestNorm * (radius - depth);
        tileColl = new TileColl(tilePos, point, bestNorm, depth);

        return true;
    }
}

struct TileColl
{
    public Vector2Int tile;
    public Vector2 point,
        normal;
    public float depth;

    public TileColl(Vector2Int tile, Vector2 point, Vector2 normal, float depth)
    {
        this.tile = tile;
        this.point = point;
        this.normal = normal;
        this.depth = depth;
    }
}
