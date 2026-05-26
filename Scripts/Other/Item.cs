using Arch.Core;
using Autofac;
using Engine.Sprites;

namespace Other;

public abstract class Item
{
    public readonly ItemInfo info;
    public abstract void Pickup(ILifetimeScope scope, Entity entity);

    public Item(ItemInfo info) => this.info = info;
}

public record struct ItemInfo(string prefabName, Sprite Sprite, string name, string description);
