using System.Numerics;
using Arch.Core;
using Components.Basic;
using Systems;

namespace Weapons.Specific;

public class Kunai : Bow
{
    public Kunai(
        BulletConfig bulletConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context
    )
        : base(bulletConfig, config, callbacks, context) { }

    protected override void OnTimer(
        Entity entity,
        Entity? extra,
        ref ModComp modComp,
        Vector2 position
    )
    {
        float step = MathF.Tau / config.maxEnemies;
        for (int i = 0; i < config.maxEnemies; i++)
        {
            float angle = i * step;
            Vector2 direction = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
            InstantiateBullet(entity, extra, ref modComp, position, direction);
        }
    }
}
