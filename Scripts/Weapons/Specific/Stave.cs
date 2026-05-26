using System.Numerics;
using Arch.Core;
using Components.Basic;
using Systems;

namespace Weapons.Specific;

public class Stave : Bow
{
    private readonly float SPREAD_ANGLE = Single.DegreesToRadians(45.0f);

    public Stave(
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
        float offset = MathF.Tau * Random.Shared.NextSingle();
        float step = SPREAD_ANGLE / config.maxEnemies;

        for (int i = 0; i < config.maxEnemies; i++)
        {
            float angle = offset + i * step;
            Vector2 dir = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
            InstantiateBullet(entity, extra, ref modComp, position, dir);
        }
    }
}
