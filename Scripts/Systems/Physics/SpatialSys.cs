using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Components.Basic;
using Components.Physics;
using Utils;

namespace Systems.Physics;

partial class SpatialSys : BaseSystem<World, float>
{
    public readonly CollisionRegistry collRegistry;
    private readonly Dictionary<Key, CachedList<Entity>> _map;

    // Max cell size is 2^(MAX_LEVELS - 1)
    // for example MAX_LEVELS=10 => maxSize = 2^9 = 512
    // max collider size is 1/2 * maxSize = 256
    private const int MAX_LEVELS = 10;

    public SpatialSys(World world)
        : base(world)
    {
        this.collRegistry = new CollisionRegistry();
        _map = new Dictionary<Key, CachedList<Entity>>();
    }

    public void GetOverlap(
        Entity entity,
        Vector2 rayStart,
        Vector2 rayEnd,
        int layer,
        CachedList<Entity> result
    )
    {
        Vector2 rayNorm = Vector2.Normalize(rayEnd - rayStart);
        rayNorm = new Vector2(-rayNorm.Y, rayNorm.X);

        Span<(int, int)> span = stackalloc (int, int)[9];
        RefSet<(int, int)> refSet = new RefSet<(int, int)>(span);

        Vector2 delta = Vector2.Normalize(rayEnd - rayStart);
        Vector2 invDelta = new Vector2(
            MathF.Abs(delta.X) > 0.00001f ? 1.0f / delta.X : 1e10f,
            MathF.Abs(delta.Y) > 0.00001f ? 1.0f / delta.Y : 1e10f
        );

        int sx = Math.Sign(delta.X);
        int sy = Math.Sign(delta.Y);

        for (int gridLevel = 0; gridLevel < MAX_LEVELS; gridLevel++)
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
                        if (!_map.TryGetValue(key, out CachedList<Entity>? list))
                            continue;

                        for (int j = 0; j < list.Count; j++)
                        {
                            Entity other = list[j];
                            if (other == entity)
                                continue;

                            ref RigidComp otherRigid = ref other.Get<RigidComp>();
                            if (layer != otherRigid.layer)
                                continue;

                            Components<CollComp, TrsComp> comps = other.Get<CollComp, TrsComp>();
                            ref CollComp otherColl = ref comps.t0;
                            ref TrsComp otherTrs = ref comps.t1;

                            Vector2 otherPosition = otherTrs.position;
                            float otherRadius = otherColl.radius * otherTrs.scale;

                            if (
                                GeomUtil.RayCircleIntersect(
                                    otherPosition,
                                    otherRadius,
                                    rayStart,
                                    rayEnd,
                                    rayNorm
                                )
                            )
                                result.Add(other);
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
        CachedList<Entity> result
    )
    {
        int minX = (int)MathF.Floor(position.X - radius);
        int minY = (int)MathF.Floor(position.Y - radius);
        int maxX = (int)MathF.Floor(position.X + radius);
        int maxY = (int)MathF.Floor(position.Y + radius);

        for (int gridLevel = 0; gridLevel < MAX_LEVELS; gridLevel++)
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

                    if (!_map.TryGetValue(key, out CachedList<Entity>? list))
                        continue;

                    for (int i = 0; i < list.Count; i++)
                    {
                        Entity other = list[i];
                        if (other == entity)
                            continue;

                        ref RigidComp otherRigid = ref other.Get<RigidComp>();
                        if (layer != otherRigid.layer)
                            continue;

                        Components<CollComp, TrsComp> comps = other.Get<CollComp, TrsComp>();
                        ref CollComp otherColl = ref comps.t0;
                        ref TrsComp otherTrs = ref comps.t1;

                        Vector2 otherPosition = otherTrs.position;
                        float otherRadius = otherColl.radius * otherTrs.scale;

                        float distSqr = Vector2.DistanceSquared(position, otherPosition);
                        float targDistSqr = (radius + otherRadius) * (radius + otherRadius);

                        if (distSqr < targDistSqr)
                            result.Add(other);
                    }
                }
            }
        }
    }

    public override void Update(in float dt)
    {
        collRegistry.Update();
        foreach (KeyValuePair<Key, CachedList<Entity>> kvp in _map)
            kvp.Value.Dispose();
        _map.Clear();

        FillQuery(World);
    }

    [Query]
    [None(typeof(DeathComp))]
    private void Fill(in Entity entity, in CollComp coll, in TrsComp trs)
    {
        Vector2 position = trs.position;
        float radius = coll.radius * trs.scale;

        Key key = GetKey(position, radius);
        if (!_map.TryGetValue(key, out CachedList<Entity>? list))
        {
            list = CachedList<Entity>.Create();
            _map[key] = list;
        }
        list.Add(entity);
    }

    private Key GetKey(Vector2 position, float radius)
    {
        int z = Math.Clamp(
            BitOperations.TrailingZeroCount(
                BitOperations.RoundUpToPowerOf2((uint)MathF.Ceiling(radius * 2.0f))
            ),
            0,
            MAX_LEVELS - 1
        );
        int x = (int)MathF.Floor(position.X) >> z;
        int y = (int)MathF.Floor(position.Y) >> z;

        return new Key(x, y, z);
    }

    private record struct Key(int x, int y, int z);
}
