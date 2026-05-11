using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Systems;
using Systems.Basic;
using Utils;

namespace Weapons.Specific;

class Laser : RayWeapon
{
    public Laser(
        RayConfig rayConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context,
        ModRegistry modRegistry
    )
        : base(rayConfig, config, callbacks, context, modRegistry) { }

    protected override void OnUpdate(Entity entity, ref ModComp modComp, Vector2 position, float dt)
    {
        base.OnUpdate(entity, ref modComp, position, dt);

        ref LineComp lineComp = ref entity.Get<LineComp>();

        float step = MathF.Tau / rayConfig.rays;
        for (int i = 0; i < rayConfig.rays; i++)
        {
            float alpha = angle + i * step;
            Vector2 dir = new Vector2(MathF.Cos(alpha), MathF.Sin(alpha));

            lineComp.lines.Add(
                new Line
                {
                    start = position + dir * 0.5f,
                    end = position + dir * rayConfig.length,
                    thick = rayConfig.thick,
                    color = rayConfig.color,
                }
            );
        }
    }

    protected override void OnTimer(Entity entity, ref ModComp modComp, Vector2 position)
    {
        using CachedList<Entity> overlap = CachedList<Entity>.Create();

        float step = MathF.Tau / rayConfig.rays;
        for (int i = 0; i < rayConfig.rays; i++)
        {
            float alpha = angle + i * step;
            Vector2 dir = new Vector2(MathF.Cos(alpha), MathF.Sin(alpha));

            context.spatial.GetOverlap(
                entity,
                position,
                position + dir * rayConfig.length,
                config.targetLayer,
                overlap
            );

            for (int j = 0; j < overlap.Count; j++)
                Damage(entity, ref modComp, overlap[j]);

            overlap.Reset();
        }
    }
}
