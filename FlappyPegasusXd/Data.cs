
using NekoRay;
using Texture = NekoRay.Texture;

namespace FlappyPegasus; 

public static class Data {
    private static Dictionary<string, Texture> _textures = new();
    private static Dictionary<string, Font> _fonts = new();

    public static Texture GetTexture(string path) {
        if (_textures.TryGetValue(path, out var texture))
            return texture;
        _textures[path] = Texture.Load(path);
        return _textures[path];
    }

    public static Font GetFont(string path, string text) {
        if (_fonts.TryGetValue(path, out var font))
            return font;
        _fonts[path] = Font.FromLove2d(path, text);
        return _fonts[path];
    }
}