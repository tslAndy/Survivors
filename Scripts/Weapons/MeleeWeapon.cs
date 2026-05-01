using System.Numerics;
using Arch.Core;
using Utils;

namespace Weapons;

class MeleeWeapon : Weapon
{
    public MeleeWeapon(WeaponConfig config, WeaponCallbacks callbacks, int targetLayer)
        : base(config, callbacks, targetLayer) { }

    protected override void OnTimer(Entity entity, Vector2 position)
    {
        CachedList<Entity> overlap = ObjectPool<CachedList<Entity>>.Shared.Get();
        spatial.GetOverlap(entity, position, config.detectRadius, targetLayer, overlap);

        for (int i = 0; i < overlap.Count; i++)
            Damage(entity, overlap[i]);

        ObjectPool<CachedList<Entity>>.Shared.Return(overlap);
    }
}
