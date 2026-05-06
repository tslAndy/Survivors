using System.Numerics;
using Arch.Buffer;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Fighting;
using Components.Other;
using Components.Physics;
using Raylib_cs;

namespace Systems.Fighting;

partial class DamageSys : BaseSystem<World, float>
{
    private readonly CommandBuffer _commandBuffer;

    public DamageSys(World world, CommandBuffer commandBuffer)
        : base(world)
    {
        _commandBuffer = commandBuffer;
    }

    [Query]
    [None(typeof(DeathComp))]
    private void UpdateDamage(
        Entity entity,
        in DamageComp damage,
        in TransformComp trs,
        ref HealthComp health
    )
    {
        int total = 0;
        for (int i = 0; i < damage.hits.Count; i++)
        {
            ref Hit hit = ref damage.hits[i];
            health.currentHP -= hit.damage;
            total += hit.damage;
        }
        if (total != 0)
            DamageNumSpawner.Spawn(World, trs.position, total);
        damage.hits.Reset();
    }

    [Query]
    private void HandleDeath(in DamageComp damage, in DeathComp death)
    {
        if (death.isDead)
            damage.hits.Dispose();
    }
}

static class DamageNumSpawner
{
    private static readonly Dictionary<int, string> _numCache = new Dictionary<int, string>();

    public static void Spawn(World world, Vector2 position, int damage)
    {
        if (!_numCache.TryGetValue(damage, out string? numStr))
        {
            numStr = damage.ToString();
            _numCache[damage] = numStr;
        }

        const float BASE_FONT_SIZE = 0.4f;

        float randOffset = (Random.Shared.NextSingle() - 0.5f) * 0.8f; // -0.4f +0.4f
        position += new Vector2(randOffset, randOffset);

        world.Create<TextComp, TransformComp, RigidComp, TimerDestroyComp>(
            new TextComp
            {
                text = numStr,
                fontSize = BASE_FONT_SIZE,
                color = Color.White,
            },
            new TransformComp { position = position, scale = 1.0f },
            new RigidComp { velocity = new Vector2(0.0f, -1.0f) },
            new TimerDestroyComp { time = 1.0f }
        );
    }
}
