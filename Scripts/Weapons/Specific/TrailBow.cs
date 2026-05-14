using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Other;
using Components.Physics;
using Raylib_cs;
using Systems;
using Systems.Physics;
using Utils;

namespace Weapons.Specific;

struct TrailConfig
{
    public float length,
        thick;
    public Color color;
}

class TrailBow : Bow
{
    protected readonly TrailConfig trailConfig;

    public TrailBow(
        TrailConfig trailConfig,
        BulletConfig bulletConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context
    )
        : base(bulletConfig, config, callbacks, context)
    {
        this.trailConfig = trailConfig;
    }

    public override void UpdateBullet(
        Entity owner,
        Entity? extra,
        Entity bullet,
        ref TrsComp trs,
        ref RigidComp rigid,
        ref CollComp coll,
        ref TimerComp timer
    )
    {
        base.UpdateBullet(owner, extra, bullet, ref trs, ref rigid, ref coll, ref timer);
        if (extra == null)
            return;

        ref LineComp lineComp = ref extra.Value.Get<LineComp>();
        using CachedList<TileColl> overlap = CachedList<TileColl>.Create();

        Vector2 pos = trs.position;
        Vector2 dir = -Vector2.Normalize(rigid.velocity);
        float len = trailConfig.length;

        for (int i = 0; i < 100; i++)
        {
            if (len <= 0.0f)
                break;

            context.tileSys.GetOverlap(pos, pos + dir * len, context.layerMap["Walls"], overlap);
            if (overlap.Count == 0)
            {
                lineComp.lines.Add(
                    new Line
                    {
                        start = pos,
                        end = pos + dir * len,
                        thick = trailConfig.thick,
                        color = trailConfig.color,
                    }
                );

                break;
            }

            ref TileColl tileColl = ref overlap[0];
            lineComp.lines.Add(
                new Line
                {
                    start = pos,
                    end = tileColl.point,
                    thick = trailConfig.thick,
                    color = trailConfig.color,
                }
            );

            len -= Vector2.Distance(tileColl.point, pos);
            pos = tileColl.point + tileColl.normal * 0.001f;
            dir = Vector2.Reflect(dir, tileColl.normal);

            overlap.Reset();
        }
    }
}
