using Engine.ResourceManagement;
using Raylib_cs;

namespace Engine.Sounds;

public class SoundManager : ResourceManager<Sound>
{
    protected override Sound Load(string path) => Raylib.LoadSound(path);

    protected override void Unload(Sound resource) => Raylib.UnloadSound(resource);
}
