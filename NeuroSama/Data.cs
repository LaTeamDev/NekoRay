using NekoLib.Filesystem;
using NekoRay;
using SoLoud;
using ZeroElectric.Vinculum;
using Font = NekoRay.Font;
using Shader = NekoRay.Shader;
using Texture = NekoRay.Texture;

namespace NeuroSama; 

public static class Data
{
    private static Dictionary<string, Texture> _textures = new();
    private static Dictionary<string, Wav> _sounds = new();
    private static Dictionary<string, Font> _fonts = new();
    private static Dictionary<string, Shader> _shaders = new();

    public static Texture GetNoTexture() {
        var path = "textures/EASTER__EGG.png";
        if (_textures.TryGetValue(path, out var texture))
            return texture;
        _textures[path] = Texture.Load(path);
        return _textures[path];
    }

    public static Texture GetTexture(string path) {
        path = path.Replace('\\', '/');
        if (_textures.TryGetValue(path, out var texture))
            return texture;
        if (!Files.FileExists(path)) return GetNoTexture();
        _textures[path] = Texture.Load(path);
        return _textures[path];
    }

    public static Font GetFont(string path, string text) {
        path = path.Replace('\\', '/');
        if (_fonts.TryGetValue(path, out var font))
            return font;
        _fonts[path] = Font.FromLove2d(path, text);
        return _fonts[path];
    }
    public static Font GetFont(string path) {
        path = path.Replace('\\', '/');
        if (_fonts.TryGetValue(path, out var font))
            return font;
        _fonts[path] = Font.Load(path);
        return _fonts[path];
    }
    
    public static Font GetFont(string path, int size, int glyphcount = 255) {
        path = path.Replace('\\', '/');
        if (_fonts.TryGetValue(path, out var font))
            return font;
        _fonts[path] = Font.Load(path, size, glyphcount);
        return _fonts[path];
    }
    
    public static Sprite GetSprite(string path) {
        path = path.Replace('\\', '/');
        var texture = GetTexture(path);
        var sprite = new Sprite(texture, new Rectangle(0, 0, texture.Width, texture.Height));
        return sprite;
    }

    public static Shader GetShader(string pathFs, string? fragVert = null) {
        pathFs = pathFs.Replace('\\', '/');
        fragVert = fragVert?.Replace('\\', '/');
        if (_shaders.TryGetValue(pathFs+fragVert, out var shader))
            return shader;
        _shaders[pathFs+fragVert] = Shader.FromFiles(pathFs, fragVert);
        return _shaders[pathFs+fragVert];
    }
    
    public static Wav GetSound(string path) {
        path = path.Replace('\\', '/');
        if (_sounds.TryGetValue(path, out var texture))
            return texture;
        using var stream = Files.GetFile(path).GetStream();
        _sounds[path] = Wav.LoadFromStream(stream);
        return _sounds[path];
    }
}