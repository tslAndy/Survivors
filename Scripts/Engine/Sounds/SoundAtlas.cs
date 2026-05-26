using Engine.ResourceManagement;
using Raylib_cs;

namespace Engine.Sounds;

public class SoundAtlas : Atlas<Sound>
{
    public SoundAtlas(Entry[] entries)
        : base(entries) { }
}
