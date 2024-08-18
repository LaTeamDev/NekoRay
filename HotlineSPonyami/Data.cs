
using NekoLib.Filesystem;
using NekoRay;
using ZeroElectric.Vinculum;
using Font = NekoRay.Font;
using Shader = NekoRay.Shader;
using Texture = NekoRay.Texture;

namespace HotlineSPonyami; 

public static class Data
{
    private static Dictionary<string, Texture> _textures = new();
    private static Dictionary<string, Font> _fonts = new();
    private static Dictionary<string, Shader> _shaders = new();

    public static Texture GetNoTexture() {
        var path = "textures/notexture.png";
        if (_textures.TryGetValue(path, out var texture))
            return texture;
        _textures[path] = Texture.Load(path);
        return _textures[path];
    }

    public static Texture GetTexture(string path) {
        if (_textures.TryGetValue(path, out var texture))
            return texture;
        if (!Files.FileExists(path)) return GetNoTexture(); // there was a possible infinite recursion, so i changed it
        _textures[path] = Texture.Load(path);
        return _textures[path];
    }

    public static Font GetFont(string path, string text) {
        if (_fonts.TryGetValue(path, out var font))
            return font;
        _fonts[path] = Font.FromLove2d(path, text);
        return _fonts[path];
    }
    
    public static Sprite GetSprite(string path) {
        var texture = GetTexture(path);
        var sprite = new Sprite(texture, new Rectangle(0, 0, texture.Width, texture.Height));
        return sprite;
    }

    public static Shader GetShader(string pathFs, string? fragVert = null) {
        if (_shaders.TryGetValue(pathFs+fragVert, out var shader))
            return shader;
        _shaders[pathFs+fragVert] = Shader.FromFiles(pathFs, fragVert);
        return _shaders[pathFs+fragVert];
    }
}