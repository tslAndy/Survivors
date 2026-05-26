using Arch.Core;
using Engine.Sprites;

namespace Other;

public abstract class Item
{
    public readonly string name,
        description;

    public readonly Sprite sprite;

    public abstract void Pickup(Entity entity);

    public Item(string name, string description, Sprite sprite)
    {
        this.name = name;
        this.description = description;
        this.sprite = sprite;
    }
}
