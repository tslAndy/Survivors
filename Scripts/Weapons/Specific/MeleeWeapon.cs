using System.Numerics;
using Arch.Core;
using Systems;
using Utils;

namespace Weapons.Specific;

class MeleeWeapon : Weapon
{
    public MeleeWeapon(WeaponConfig config, WeaponCallbacks callbacks, WorldContext context)
        : base(config, callbacks, context) { }

    protected override void OnTimer(Entity entity, Vector2 position)
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
            Damage(entity, overlap[i]);
    }
}
