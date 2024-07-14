using ZeroElectric.Vinculum;
using Texture = NekoRay.Texture;

namespace FlappyPegasus; 

public static class Data {
    private static Dictionary<string, Texture> _textures = new();

    public static Texture GetTexture(string path) {
        if (_textures.TryGetValue(path, out var texture))
            return texture;
        _textures[path] = Texture.Load(path);
        return _textures[path];
    }
}