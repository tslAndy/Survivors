using Engine.ResourceManagement;

namespace Engine.Animations;

class AnimAtlas : Atlas<Anim>
{
    public AnimAtlas(Entry[] entries, Group[]? groups = null)
        : base(entries, groups) { }
}
