using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Raylib_cs;
using Systems;
using Systems.Physics;
using Utils;

namespace Weapons.Specific;

struct LaserConfig
{
    public int raysCount;
    public float start,
        end,
        thick,
        rotSpeed;
    public Color color;
}

class LaserWeapon : Weapon
{
    protected readonly LaserConfig laserConfig;

    public LaserWeapon(
        LaserConfig laserConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context
    )
        : base(config, callbacks, context)
    {
        this.laserConfig = laserConfig;
    }

    protected override void OnUpdate(
        Entity entity,
        Entity? extra,
        ref ModComp modComp,
        Vector2 position,
        float dt
    )
    {
        if (extra == null)
            return;

        ref LineComp lineComp = ref extra.Value.Get<LineComp>();
        lineComp.lines.Reset();

        ref LocalTrsComp localTrs = ref extra.Value.Get<LocalTrsComp>();

        using CachedList<TileColl> overlap = CachedList<TileColl>.Create();

        localTrs.rotation += dt * laserConfig.rotSpeed;

        float step = 360.0f / laserConfig.raysCount;
        for (int i = 0; i < laserConfig.raysCount; i++)
        {
            float angle = Single.DegreesToRadians(localTrs.rotation + i * step);
            Vector2 dir = new Vector2(MathF.Cos(angle), MathF.Sin(angle));

            Vector2 start = position + dir * laserConfig.start;
            Vector2 end = position + dir * laserConfig.end;

            Line line = new Line
            {
                start = start,
                end = end,
                thick = laserConfig.thick,
                color = laserConfig.color,
            };

            context.tileSys.GetOverlap(start, end, context.layerMap["Walls"], overlap);
            if (overlap.Count != 0)
            {
                ref TileColl coll = ref overlap[0];
                line.end = coll.point;
            }

            lineComp.lines.Add(line);
            overlap.Reset();
        }
    }

    protected override void OnTimer(
        Entity entity,
        Entity? extra,
        ref ModComp modComp,
        Vector2 position
    )
    {
        if (extra == null)
            return;

        ref ModComp mod = ref entity.Get<ModComp>();
        ref LineComp lineComp = ref extra.Value.Get<LineComp>();

        using CachedList<Entity> overlap = CachedList<Entity>.Create();

        for (int i = 0; i < lineComp.lines.Count; i++)
        {
            ref Line line = ref lineComp.lines[i];
            context.spatialSys.GetOverlap(
                entity,
                line.start,
                line.end,
                config.targetLayer,
                overlap
            );

            for (int j = 0; j < overlap.Count; j++)
                Damage(entity, ref mod, overlap[j]);

            overlap.Reset();
        }
    }
}
