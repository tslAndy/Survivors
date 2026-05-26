using System.Numerics;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Physics;
using Utils;

namespace Systems.Physics;

public partial class SpatialSys : BaseSystem<World, float>
{
    public readonly CollisionRegistry collRegistry;
    private readonly Dictionary<Key, CachedList<Entry>> _map;

    // Max cell size is 2^(MAX_LEVELS - 1)
    // for example MAX_LEVELS=5 => maxSize = 2^(5-1) = 16
    // max collider size is 1/2 * maxSize = 8
    private const int MIN_LEVEL = 1;
    private const int MAX_LEVEL = 5;

    public SpatialSys(World world)
        : base(world)
    {
        this.collRegistry = new CollisionRegistry();
        _map = new Dictionary<Key, CachedList<Entry>>();
    }

    public void GetOverlap(
        Entity entity,
        Vector2 rayStart,
        Vector2 rayEnd,
        int layer,
        CachedList<Entity> overlap
    )
    {
        Vector2 rayNorm = Vector2.Normalize(rayEnd - rayStart);
        rayNorm = new Vector2(-rayNorm.Y, rayNorm.X);

        Span<(int, int)> span = stackalloc (int, int)[9];
        RefSet<(int, int)> refSet = new RefSet<(int, int)>(span);

        Vector2 delta = rayEnd - rayStart;
        if (delta.LengthSquared() < 0.001f * 0.001f)
            return;

        delta = Vector2.Normalize(delta);

        Vector2 invDelta = new Vector2(
            MathF.Abs(delta.X) > 0.00001f ? 1.0f / delta.X : 1e10f,
            MathF.Abs(delta.Y) > 0.00001f ? 1.0f / delta.Y : 1e10f
        );

        int sx = Math.Sign(delta.X);
        int sy = Math.Sign(delta.Y);

        for (int gridLevel = MIN_LEVEL; gridLevel < MAX_LEVEL; gridLevel++)
        {
            refSet.Reset();

            float cellSize = 1 << gridLevel;
            Vector2 localStart = rayStart / cellSize;
            Vector2 localEnd = rayEnd / cellSize;

            Vector2 pos = localStart;

            int cx = sx > 0 ? (int)MathF.Floor(pos.X) : (int)MathF.Ceiling(pos.X);
            int cy = sy > 0 ? (int)MathF.Floor(pos.Y) : (int)MathF.Ceiling(pos.Y);

            Vector2 min = Vector2.Min(localStart, localEnd);
            Vector2 max = Vector2.Max(localStart, localEnd);

            for (int i = 0; i < 10_000; i++)
            {
                if (Vector2.LessThanAny(pos, min) || Vector2.GreaterThanAny(pos, max))
                    break;

                int px = (int)MathF.Floor(pos.X);
                int py = (int)MathF.Floor(pos.Y);

                for (int ky = py - 1; ky < py + 2; ky++)
                {
                    for (int kx = px - 1; kx < px + 2; kx++)
                    {
                        if (!refSet.TryAdd((kx, ky)))
                            continue;

                        Key key = new Key(kx, ky, gridLevel);
                        if (!_map.TryGetValue(key, out CachedList<Entry>? list))
                            continue;

                        for (int j = 0; j < list.Count; j++)
                        {
                            Entry entry = list[j];
                            if (entry.entity == entity)
                                continue;

                            if (layer != entry.layer)
                                continue;

                            if (
                                GeomUtil.RayCircleIntersect(
                                    entry.position,
                                    entry.radius,
                                    rayStart,
                                    rayEnd,
                                    rayNorm
                                )
                            )
                            {
                                overlap.Add(entry.entity);
                            }
                        }
                    }
                }

                Vector2 t = new Vector2(cx + sx - pos.X, cy + sy - pos.Y) * invDelta;
                t.X += 0.0001f;
                t.Y += 0.0001f;

                if (t.X > 0.0f && (t.X < t.Y || t.Y < 0.0f))
                {
                    pos += t.X * delta;
                    cx += sx;
                }
                else if (t.Y > 0.0f && (t.Y < t.X || t.X < 0.0f))
                {
                    pos += t.Y * delta;
                    cy += sy;
                }
                else
                {
                    break;
                }
            }
        }
    }

    public void GetOverlap(
        Entity entity,
        Vector2 position,
        float radius,
        int layer,
        CachedList<Entity> overlap
    )
    {
        int minX = (int)MathF.Floor(position.X - radius);
        int minY = (int)MathF.Floor(position.Y - radius);
        int maxX = (int)MathF.Floor(position.X + radius);
        int maxY = (int)MathF.Floor(position.Y + radius);

        for (int gridLevel = MIN_LEVEL; gridLevel < MAX_LEVEL; gridLevel++)
        {
            int xStart = (minX >> gridLevel) - 1;
            int yStart = (minY >> gridLevel) - 1;
            int xEnd = (maxX >> gridLevel) + 2;
            int yEnd = (maxY >> gridLevel) + 2;

            for (int y = yStart; y < yEnd; y++)
            {
                for (int x = xStart; x < xEnd; x++)
                {
                    Key key = new Key(x, y, gridLevel);

                    if (!_map.TryGetValue(key, out CachedList<Entry>? list))
                        continue;

                    for (int i = 0; i < list.Count; i++)
                    {
                        Entry entry = list[i];
                        if (entry.entity == entity)
                            continue;

                        if (layer != entry.layer)
                            continue;

                        float distSqr = Vector2.DistanceSquared(position, entry.position);
                        float targDistSqr = (radius + entry.radius) * (radius + entry.radius);

                        if (distSqr < targDistSqr)
                            overlap?.Add(entry.entity);
                    }
                }
            }
        }
    }

    public override void Update(in float dt)
    {
        collRegistry.Update();
        foreach (KeyValuePair<Key, CachedList<Entry>> kvp in _map)
            kvp.Value.Dispose();
        _map.Clear();

        FillQuery(World);
    }

    [Query]
    [None(typeof(DeathComp))]
    private void Fill(in Entity entity, in TrsComp trs, in CollComp coll, in RigidComp rigid)
    {
        Vector2 position = trs.position;
        float radius = coll.radius * trs.scale;

        Key key = GetKey(position, radius);
        if (!_map.TryGetValue(key, out CachedList<Entry>? list))
        {
            list = CachedList<Entry>.Create();
            _map[key] = list;
        }
        list.Add(new Entry(entity, position, radius, rigid.layer));
    }

    private Key GetKey(Vector2 position, float radius)
    {
        int z = Math.Clamp(
            BitOperations.TrailingZeroCount(
                BitOperations.RoundUpToPowerOf2((uint)MathF.Ceiling(radius * 2.0f))
            ),
            MIN_LEVEL,
            MAX_LEVEL
        );
        int x = (int)MathF.Floor(position.X) >> z;
        int y = (int)MathF.Floor(position.Y) >> z;

        return new Key(x, y, z);
    }

    private record struct Entry(Entity entity, Vector2 position, float radius, int layer);

    private record struct Key(int x, int y, int z);
}
