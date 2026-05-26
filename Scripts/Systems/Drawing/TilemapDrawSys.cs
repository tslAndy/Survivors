using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Components.Other;
using Engine.Common;
using Engine.Tilemaps;
using Raylib_cs;
using Utils;

namespace Systems.Drawing;

public partial class TilemapDrawSys : BaseSystem<World, float>
{
    private readonly CachedList<TilemapItem> _tilemapItems;
    private readonly IComparer<TilemapItem> _comparer;
    private readonly Camera _cam;

    public TilemapDrawSys(World world, Camera cam)
        : base(world)
    {
        _tilemapItems = CachedList<TilemapItem>.Create();
        _comparer = Comparer<TilemapItem>.Create((a, b) => a.order.CompareTo(b.order));
        _cam = cam;
    }

    public override void Update(in float dt)
    {
        FillTilemapsQuery(World);
        DrawTilemaps();
    }

    [Query]
    private void FillTilemaps(Entity entity, in TilemapComp tilemapComp)
    {
        _tilemapItems.Add(new TilemapItem(entity, tilemapComp.drawOrder));
    }

    private void DrawTilemaps()
    {
        Array.Sort(_tilemapItems.Arr, 0, _tilemapItems.Count, _comparer);

        (int sx, int ex, int sy, int ey) = GetFrustChunks();

        for (int i = 0; i < _tilemapItems.Count; i++)
        {
            Entity entity = _tilemapItems[i].entity;
            Tilemap tilemap = entity.Get<TilemapComp>().tilemap;

            for (int cy = sy; cy < ey; cy++)
            {
                for (int cx = sx; cx < ex; cx++)
                {
                    TileChunk? chunk = tilemap.GetChunk(cx, cy);
                    if (chunk != null)
                    {
                        DrawChunk(chunk, new Vector2(cx, cy) * TileChunk.SIZE);
                    }
                }
            }
        }

        _tilemapItems.Reset();
    }

    private void DrawChunk(TileChunk chunk, Vector2 chunkPos)
    {
        Vector2 basePos = chunkPos - _cam.frustum.min;

        for (int ty = 0; ty < TileChunk.SIZE; ty++)
        {
            for (int tx = 0; tx < TileChunk.SIZE; tx++)
            {
                Tile? tile = chunk[tx, ty];
                if (tile == null)
                    continue;

                // translate to world coords
                Vector2 position = basePos + new Vector2(tx, ty);
                Vector2 size = 0.0625f * tile.rect.Size;

                // translate to screen space
                position *= _cam.pixelsPerUnit;
                size *= _cam.pixelsPerUnit;

                Rectangle dest = new Rectangle(position, size);

                Raylib.DrawTexturePro(
                    tile.texture,
                    tile.rect,
                    dest,
                    Vector2.Zero,
                    0.0f,
                    Color.White
                );
            }
        }
    }

    private (int, int, int, int) GetFrustChunks()
    {
        AABB camBox = _cam.frustum;
        int minX = (int)MathF.Floor(camBox.min.X);
        int minY = (int)MathF.Floor(camBox.min.Y);
        int maxX = (int)MathF.Ceiling(camBox.max.X);
        int maxY = (int)MathF.Ceiling(camBox.max.Y);

        int sx = minX / TileChunk.SIZE - ((minX % TileChunk.SIZE == 0) ? 0 : 1);
        int ex = maxX / TileChunk.SIZE + ((maxX % TileChunk.SIZE == 0) ? 0 : 1);

        int sy = minY / TileChunk.SIZE - ((minY % TileChunk.SIZE == 0) ? 0 : 1);
        int ey = maxY / TileChunk.SIZE + ((maxY % TileChunk.SIZE == 0) ? 0 : 1);

        return (sx, ex, sy, ey);
    }

    private struct TilemapItem
    {
        public Entity entity;
        public int order;

        public TilemapItem(Entity entity, int order)
        {
            this.entity = entity;
            this.order = order;
        }
    }
}
