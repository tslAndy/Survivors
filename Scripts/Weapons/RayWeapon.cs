using System.Numerics;
using Arch.Core;
using Components.Basic;
using Raylib_cs;
using Systems;

namespace Weapons;

struct RayConfig
{
    public int rays;
    public float length,
        rotationSpeed;

    public float thick;
    public Color color;
}

abstract class RayWeapon : Weapon
{
    private float _angle;
    protected float angle => _angle;

    protected readonly RayConfig rayConfig;

    protected RayWeapon(
        RayConfig rayConfig,
        WeaponConfig config,
        WeaponCallbacks callbacks,
        WorldContext context
    )
        : base(config, callbacks, context)
    {
        this.rayConfig = rayConfig;
    }

    protected override void OnUpdate(Entity entity, ref ModComp modComp, Vector2 position, float dt)
    {
        _angle += rayConfig.rotationSpeed * dt;
    }
}
