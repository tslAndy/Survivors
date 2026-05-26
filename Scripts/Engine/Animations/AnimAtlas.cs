using Engine.ResourceManagement;

namespace Engine.Animations;

public class AnimAtlas : Atlas<Anim>
{
    public AnimAtlas(Entry[] entries, Group[]? groups = null)
        : base(entries, groups) { }
}
