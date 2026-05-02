using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Components.Fighting;

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

abstract class Weapon
{
    private float time;

    protected readonly WeaponConfig config;
    protected readonly WeaponCallbacks callbacks;
    protected readonly WeaponContext context;

    protected Weapon(WeaponConfig config, WeaponCallbacks callbacks, WeaponContext context)
    {
        this.config = config;
        this.callbacks = callbacks;

        this.time = 0.0f;
        this.context = context;
    }

    public void Update(Entity entity, Vector2 position, float dt)
    {
        OnUpdate(entity, position, dt);

        time += dt;
        if (time < config.attackTime)
            return;

        time -= config.attackTime;
        OnTimer(entity, position);
    }

    protected virtual void OnUpdate(Entity entity, Vector2 position, float dt) { }

    protected virtual void OnTimer(Entity entity, Vector2 position) { }

    protected void Damage(Entity source, Entity target)
    {
        ref DamageComp damageComp = ref target.Get<DamageComp>();

        // count crit chance
        float critChance = config.critChance;
        callbacks.getCritChance?.Invoke(source, target, ref critChance);

        if (Random.Shared.Next(0, 100) < critChance)
        {
            float critDamage = config.critDamage;

            // apply both base and crit damage modifiers
            callbacks.countBaseDamage?.Invoke(source, target, ref critDamage);
            callbacks.countCritDamage?.Invoke(source, target, ref critDamage);
            damageComp.hits.Add(new Hit(source, (int)MathF.Floor(critDamage), true));

            // apply both base and crit actions
            callbacks.onBaseDamage?.Invoke(source, target, ref critDamage);
            callbacks.onCritDamage?.Invoke(source, target, ref critDamage);
        }
        else
        {
            float baseDamage = config.baseDamage;

            // apply only base action
            callbacks.countBaseDamage?.Invoke(source, target, ref baseDamage);
            damageComp.hits.Add(new Hit(source, (int)MathF.Floor(baseDamage), false));

            // apply base damage action
            callbacks.onBaseDamage?.Invoke(source, target, ref baseDamage);
        }
    }
}
