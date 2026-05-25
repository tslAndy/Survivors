using Engine.ResourceManagement;
using Raylib_cs;

namespace Engine.Sounds;

class MusicManager : ResourceManager<Music>
{
    protected override Music Load(string path) => Raylib.LoadMusicStream(path);

    protected override void Unload(Music resource) => Raylib.UnloadMusicStream(resource);
}
