using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Raylib_cs;
using Systems;
using Systems.Basic;
using Systems.Physics;
using Utils;

namespace Weapons.Specific;

class MeleeWeapon : Weapon
{
    public MeleeWeapon(
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context,
        ModRegistry modRegistry
    )
        : base(config, callbacks, context, modRegistry) { }

    protected override void OnUpdate(Entity entity, ref ModComp modComp, Vector2 position, float dt)
    {
        ref LineComp lineComp = ref entity.Get<LineComp>();
        lineComp.lines.Reset();

        using CachedList<TileColl> colls = CachedList<TileColl>.Create();

        for (int i = 0; i < 24; i++)
        {
            Vector2 delta = new Vector2(
                MathF.Cos(Single.DegreesToRadians(i * 15.0f)),
                MathF.Sin(Single.DegreesToRadians(i * 15.0f))
            );

            lineComp.lines.Add(
                new Line
                {
                    start = position,
                    end = position + delta * 3,
                    thick = 0.2f,
                    color = Color.Green,
                }
            );

            context.tileCollSys.GetOverlap(
                position,
                position + delta * 20,
                context.layerMap["Walls"],
                colls
            );
        }

        for (int i = 0; i < colls.Count; i++)
        {
            ref TileColl coll = ref colls[i];
            lineComp.lines.Add(
                new Line
                {
                    start = position,
                    end = coll.point,
                    thick = 0.1f,
                    color = Color.Red,
                }
            );
        }
    }

    protected override void OnTimer(Entity entity, ref ModComp modComp, Vector2 position)
    {
        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        context.spatial.GetOverlap(
            entity,
            position,
            config.detectRadius,
            config.targetLayer,
            overlap
        );

        for (int i = 0; i < overlap.Count; i++)
            Damage(entity, ref modComp, overlap[i]);
    }
}
