using ZeroElectric.Vinculum.Extensions;

namespace NekoRay; 

public class Font : IDisposable {
    internal RayFont _font;

    internal Font(RayFont font) {
        _font = font;
        Texture = new Texture {
            _texture = _font.texture
        };
    }

    public bool IsReady => Raylib.IsFontReady(_font);

    public Texture Texture;

    public static readonly Font Default = new(Raylib.GetFontDefault());

    public static Font Load(string fileName) => new(Raylib.LoadFont(fileName));

    public static unsafe Font Load(string fileName, int fontSize, int[] codepoints) {
        var pin = codepoints.GcPin();
        var font = new Font(Raylib.LoadFontEx(fileName, fontSize, (int*)pin.AddrOfPinnedObject(), codepoints.Length));
        pin.Free();
        return font;
    }

    public static Font FromImage(Image image, Color key, int firstChar) =>
        new (Raylib.LoadFontFromImage(image._image, key, firstChar));

    public void Dispose() {
        Raylib.UnloadFont(_font);
        Texture = null;
    }
}