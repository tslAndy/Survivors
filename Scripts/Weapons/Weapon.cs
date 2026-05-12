using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Basic;
using Components.Fighting;
using Engine.Common;
using Systems;
using Systems.Basic;

namespace Weapons;

struct WeaponConfig
{
    public int targetLayer;

    public int baseDamage,
        critDamage;
    public int critChance;

    public float attackTime;
    public float detectRadius;
}

struct WeaponCallbacks
{
    public delegate void Callback(Entity attacker, Entity target, ref float val);
    public Callback? getCritChance,
        countBaseDamage,
        countCritDamage,
        onBaseDamage,
        onCritDamage;
}

class Weapon : IWeapon
{
    private float _time;

    protected readonly WeaponConfig config;
    protected readonly WeaponCallbacks callbacks;
    protected readonly WorldContext context;

    protected readonly Hash DamageGiveHash = ModRegistry.CountHash("damageGiveFactor");
    protected readonly Hash DetectRadiusHash = ModRegistry.CountHash("detectRadiusFactor");

    public Weapon(WeaponConfig config, WeaponCallbacks callbacks, WorldContext context)
    {
        this.config = config;
        this.callbacks = callbacks;
        this.context = context;

        this._time = 0.0f;
    }

    public void Update(Entity entity, ref ModComp modComp, Vector2 position, float dt)
    {
        OnUpdate(entity, ref modComp, position, dt);

        _time += dt;
        if (_time < config.attackTime)
            return;

        _time -= config.attackTime;
        OnTimer(entity, ref modComp, position);
    }

    protected virtual void OnUpdate(
        Entity entity,
        ref ModComp modComp,
        Vector2 position,
        float dt
    ) { }

    protected virtual void OnTimer(Entity entity, ref ModComp modComp, Vector2 position) { }

    protected void Damage(Entity source, ref ModComp modComp, Entity target)
    {
        ref DamageComp damageComp = ref target.Get<DamageComp>();

        // count crit chance
        float damageFactor = modComp[DamageGiveHash];
        float critChance = config.critChance;
        callbacks.getCritChance?.Invoke(source, target, ref critChance);

        if (Random.Shared.Next(0, 100) < critChance)
        {
            float critDamage = config.critDamage * damageFactor;

            // apply both base and crit damage modifiers
            callbacks.countBaseDamage?.Invoke(source, target, ref critDamage);
            callbacks.countCritDamage?.Invoke(source, target, ref critDamage);
            damageComp.hits.Add(new Hit(source, (int)MathF.Floor(critDamage)));

            // apply both base and crit actions
            callbacks.onBaseDamage?.Invoke(source, target, ref critDamage);
            callbacks.onCritDamage?.Invoke(source, target, ref critDamage);
        }
        else
        {
            float baseDamage = config.baseDamage * damageFactor;

            // apply only base action
            callbacks.countBaseDamage?.Invoke(source, target, ref baseDamage);
            damageComp.hits.Add(new Hit(source, (int)MathF.Floor(baseDamage)));

            // apply base damage action
            callbacks.onBaseDamage?.Invoke(source, target, ref baseDamage);
        }
    }
}
