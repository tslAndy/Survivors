using Engine.ResourceManagement;

namespace Engine.Sprites;

public class SpriteAtlas : Atlas<Sprite>
{
    public SpriteAtlas(Entry[] entries, Group[]? groups)
        : base(entries, groups) { }
}
