using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Components.Basic;
using Components.Loot;
using Components.Other;
using Systems.Physics;
using Utils;

namespace Systems.Loot;

partial class LootCollectSys : BaseSystem<World, float>
{
    private readonly SpatialSys _spatial;
    private readonly int _lootLayer;

    public LootCollectSys(World world, SpatialSys spatial, int lootLayer)
        : base(world)
    {
        _spatial = spatial;
        _lootLayer = lootLayer;
    }

    [Query]
    private void CollectLoot(
        [Data] in float dt,
        Entity entity,
        ref LootCollComp lootColl,
        ref TransformComp trs
    )
    {
        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        _spatial.GetOverlap(entity, trs.position, lootColl.radius, _lootLayer, overlap);

        for (int i = 0; i < overlap.Count; i++)
        {
            Entity targ = overlap[i];

            ref TransformComp targTrs = ref targ.Get<TransformComp>();

            if (Vector2.DistanceSquared(trs.position, targTrs.position) > 0.0001f)
            {
                targTrs.position +=
                    dt * lootColl.speed * Vector2.Normalize(trs.position - targTrs.position);
                continue;
            }

            Components<LootComp, TimerDestroyComp> comps = targ.Get<LootComp, TimerDestroyComp>();
            ref LootComp targLoot = ref comps.t0;
            ref TimerDestroyComp targTimerDestroy = ref comps.t1;

            lootColl.amount += targLoot.amount;
            targTimerDestroy.time = 0.0f;
        }
    }
}
