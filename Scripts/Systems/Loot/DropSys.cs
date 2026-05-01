using System.Numerics;
using Arch.Core;
using Arch.System;
using Components.Basic;
using Components.Loot;
using Components.Other;
using Components.Physics;
using Engine.Common;
using Engine.Sprites;
using Utils;

namespace Systems.Fighting;

partial class DropSys : BaseSystem<World, float>
{
    private Hash THOUSAND_HASH = SpriteAtlas.CountHash("money_thousand");
    private Hash HUNDRED_HASH = SpriteAtlas.CountHash("money_hundred");
    private Hash TEN_HASH = SpriteAtlas.CountHash("money_ten");
    private Hash ONE_HASH = SpriteAtlas.CountHash("money_one");

    private readonly SpriteAtlas _itemsAtlas;

    public DropSys(World world)
        : base(world)
    {
        _itemsAtlas = ServiceLocator
            .Get<SpriteAtlasManager>()
            .Get("./Resources/SpriteAtlases/Items/MainItems.spriteAtlas");
    }

    [Query]
    [All(typeof(DeathComp))]
    private void UpdateDrop(in DropComp drop, in AnimComp animator, in TransformComp trs)
    {
        if (!animator.isFinished)
            return;

        int amount = drop.amount;

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

        World.Create<TransformComp, RigidComp, CollComp, SpriteComp, LootComp, TimerDestroyComp>(
            new TransformComp { position = position + offset, scale = 0.5f },
            new RigidComp { layer = (int)Layers.Loot },
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
