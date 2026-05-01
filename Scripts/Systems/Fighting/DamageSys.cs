using System.Numerics;
using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Components.Other;
using Components.Physics;
using Raylib_cs;
using Utils;

namespace Systems.Fighting;

partial class DamageSys : BaseSystem<World, float>
{
    private readonly CommandBuffer _commBuffer;

    public DamageSys(World world)
        : base(world)
    {
        _commBuffer = ServiceLocator.Get<CommandBuffer>();
    }

    [Query]
    [None(typeof(DeathComp))]
    private void UpdateDamage(
        Entity entity,
        in DamageComp damage,
        ref HealthComp health,
        in TransformComp trs
    )
    {
        for (int i = 0; i < damage.hits.Count; i++)
        {
            ref Hit hit = ref damage.hits[i];
            if (hit.damage == 0)
                continue;
            health.currentHP -= hit.damage;
            DamageNumSpawner.Spawn(World, trs.position, hit.damage, hit.isCrit);
        }

        damage.hits.Reset();
    }
}

static class DamageNumSpawner
{
    private static readonly Dictionary<int, string> _numCache = new Dictionary<int, string>();

    public static void Spawn(World world, Vector2 position, int damage, bool isCrit)
    {
        if (!_numCache.TryGetValue(damage, out string? numStr))
        {
            numStr = damage.ToString();
            _numCache[damage] = numStr;
        }

        const float BASE_FONT_SIZE = 0.4f;
        const float CRIT_FONT_SIZE = 0.7f;

        float randOffset = (Random.Shared.NextSingle() - 0.5f) * 0.8f; // -0.4f +0.4f
        position += new Vector2(randOffset, randOffset);

        world.Create<TextComp, TransformComp, RigidComp, TimerDestroyComp>(
            new TextComp
            {
                text = numStr,
                fontSize = isCrit ? CRIT_FONT_SIZE : BASE_FONT_SIZE,
                color = isCrit ? Color.Red : Color.White,
            },
            new TransformComp { position = position, scale = 1.0f },
            new RigidComp { velocity = new Vector2(0.0f, -1.0f) },
            new TimerDestroyComp { time = 1.0f }
        );
    }
}
