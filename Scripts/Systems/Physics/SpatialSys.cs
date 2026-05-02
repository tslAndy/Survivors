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

                        Components<CollComp, TransformComp> comps = other.Get<
                            CollComp,
                            TransformComp
                        >();
                        ref CollComp otherColl = ref comps.t0;
                        ref TransformComp otherTrs = ref comps.t1;

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
    private void Fill(in Entity entity, in CollComp coll, in TransformComp trs)
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
