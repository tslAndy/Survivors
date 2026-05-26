using System.Numerics;
using Arch.Bus;
using Arch.Core;
using Components.Basic;
using Components.Other;
using Components.Physics;
using Events;
using Raylib_cs;

namespace UI;

public partial class DamageUI
{
    private readonly Dictionary<int, string> _numCache;
    private World _world;

    const float BASE_FONT_SIZE = 0.4f;

    public DamageUI(World world)
    {
        _numCache = new Dictionary<int, string>();
        _world = world;
        Hook();
    }

    [Event]
    public void OnDamage(ref DamageEvent @event)
    {
        int damage = @event.damage;
        bool positive = damage > 0;

        damage = Math.Abs(damage);
        if (!_numCache.TryGetValue(damage, out string? numStr))
        {
            numStr = damage.ToString();
            _numCache[damage] = numStr;
        }

        Vector2 position = @event.position;
        float randOffset = (Random.Shared.NextSingle() - 0.5f) * 0.8f; // -0.4f +0.4f
        position += new Vector2(randOffset, randOffset);

        _world.Create<TextComp, TrsComp, RigidComp, TimerComp>(
            new TextComp
            {
                text = numStr,
                fontSize = BASE_FONT_SIZE,
                color = positive ? Color.Red : Color.Green,
            },
            new TrsComp { position = position, scale = 1.0f },
            new RigidComp { velocity = new Vector2(0.0f, -1.0f) },
            new TimerComp { time = 1.0f }
        );
    }
}
