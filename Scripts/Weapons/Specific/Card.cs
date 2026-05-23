using System.Numerics;
using Arch.Core;
using Components.Basic;
using Systems;

namespace Weapons.Specific;

class Card : Book
{
    public Card(
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
        for (int i = 0; i < config.maxEnemies; i++)
        {
            float angle = i * 2.399963f;
            float t = MathF.Sqrt((float)i / config.maxEnemies);
            float dist = config.detectRadius * t;
            Vector2 dir = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
            InstantiateBullet(entity, extra, ref modComp, position + dir * dist, Vector2.Zero);
        }
    }
}
