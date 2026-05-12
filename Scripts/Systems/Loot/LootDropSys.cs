using System.Numerics;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Loot;
using Components.Other;
using Components.Physics;
using Engine.Common;
using Engine.Sprites;
using Systems.Basic;

namespace Systems.Loot;

partial class LootDropSys : BaseSystem<World, float>
{
    private readonly Hash THOUSAND_HASH = SpriteAtlas.CountHash("money_thousand");
    private readonly Hash HUNDRED_HASH = SpriteAtlas.CountHash("money_hundred");
    private readonly Hash TEN_HASH = SpriteAtlas.CountHash("money_ten");
    private readonly Hash ONE_HASH = SpriteAtlas.CountHash("money_one");

    private readonly Hash LootDropHash = ModRegistry.CountHash("lootDropFactor");

    private readonly SpriteAtlas _itemsAtlas;
    private readonly int _lootLayer;

    public LootDropSys(World world, SpriteAtlas itemsAtlas, LayerMap layerMap)
        : base(world)
    {
        _itemsAtlas = itemsAtlas;
        _lootLayer = layerMap["Loot"];
    }

    [Query]
    private void UpdateDrop(in DeathComp death, in DropComp drop, in ModComp mod, in TrsComp trs)
    {
        if (!death.isDead)
            return;

        int amount = (int)MathF.Floor(drop.amount * mod[LootDropHash]);

        int thousands = amount - (amount % 1000);
        if (thousands != 0)
            Create(thousands, trs.position, THOUSAND_HASH);
        amount -= thousands;

        int hundreds = amount - (amount % 100);
        if (hundreds != 0)
            Create(hundreds, trs.position, HUNDRED_HASH);
        amount -= hundreds;

        int tens = amount - (amount % 10);
        if (tens != 0)
            Create(tens, trs.position, TEN_HASH);
        amount -= tens;

        int ones = amount;
        if (ones != 0)
            Create(ones, trs.position, ONE_HASH);
    }

    private void Create(int amount, Vector2 position, Hash groupHash)
    {
        Vector2 offset = new Vector2(
            Random.Shared.NextSingle() - 0.5f,
            Random.Shared.NextSingle() - 0.5f
        );

        World.Create<TrsComp, RigidComp, CollComp, SpriteComp, LootComp, TimerDestroyComp>(
            new TrsComp { position = position + offset, scale = 0.5f },
            new RigidComp { layer = _lootLayer },
            new CollComp { radius = 1.0f },
            new SpriteComp
            {
                sprite = _itemsAtlas[
                    groupHash,
                    Random.Shared.Next(_itemsAtlas.GetGroupLength(groupHash))
                ],
                drawOrder = 0,
            },
            new LootComp { amount = amount },
            new TimerDestroyComp { time = 10.0f }
        );
    }
}
