using Engine.ResourceManagement;
using Raylib_cs;

namespace Engine.Sounds;

class SoundManager : ResourceManager<Sound>
{
    public SoundManager()
        : base(path => Raylib.LoadSound(path), sound => Raylib.UnloadSound(sound)) { }
}
