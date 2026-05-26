using Arch.Core;

namespace Other;

public abstract class Item
{
    public readonly string name,
        description;

    public abstract void Pickup(Entity entity);

    public Item(string name, string description)
    {
        this.name = name;
        this.description = description;
    }
}
