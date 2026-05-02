using System.Numerics;
using Arch.Core;
using Utils;

namespace Weapons.Specific;

class MeleeWeapon : Weapon
{
    public MeleeWeapon(WeaponConfig config, WeaponCallbacks callbacks, int targetLayer)
        : base(config, callbacks, targetLayer) { }

    protected override void OnTimer(Entity entity, Vector2 position)
    {
        using CachedList<Entity> overlap = CachedList<Entity>.Create();
        spatial.GetOverlap(entity, position, config.detectRadius, targetLayer, overlap);

        for (int i = 0; i < overlap.Count; i++)
            Damage(entity, overlap[i]);
    }
}
