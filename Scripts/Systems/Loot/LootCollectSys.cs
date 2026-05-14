using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;
using Components.Basic;
using Components.Loot;
using Components.Other;
using Engine.Common;
using Systems.Basic;
using Systems.Physics;
using Utils;

namespace Systems.Loot;

partial class LootCollectSys : BaseSystem<World, float>
{
    private readonly SpatialSys _spatial;
    private readonly int _lootLayer;

    private readonly Hash IncomeFactorHash = ModRegistry.CountHash("lootIncomeFactor"),
        IncomeRadiusHash = ModRegistry.CountHash("lootRadiusFactor");

    public LootCollectSys(World world, SpatialSys spatial, LayerMap layerMap)
        : base(world)
    {
        _spatial = spatial;
        _lootLayer = layerMap["Loot"];
    }

    [Query]
    private void CollectLoot(
        [Data] in float dt,
        Entity entity,
        ref LootCollComp lootColl,
        ref TrsComp trs,
        ref ModComp modComp
    )
    {
        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        _spatial.GetOverlap(
            entity,
            trs.position,
            lootColl.radius * modComp[IncomeRadiusHash],
            _lootLayer,
            overlap
        );

        float totalAmount = 0.0f;
        for (int i = 0; i < overlap.Count; i++)
        {
            Entity targ = overlap[i];
            ref TrsComp targTrs = ref targ.Get<TrsComp>();
            if (Vector2.DistanceSquared(trs.position, targTrs.position) > 0.0001f)
            {
                targTrs.position +=
                    dt * lootColl.speed * Vector2.Normalize(trs.position - targTrs.position);
                continue;
            }

            Components<LootComp, TimerComp> comps = targ.Get<LootComp, TimerComp>();

            ref LootComp targLoot = ref comps.t0;
            totalAmount += targLoot.amount;

            ref TimerComp targTimerDestroy = ref comps.t1;
            targTimerDestroy.time = 0.0f;
        }

        lootColl.amount += (int)MathF.Floor(totalAmount * modComp[IncomeFactorHash]);
    }
}
