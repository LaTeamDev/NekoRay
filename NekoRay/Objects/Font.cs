using System.Numerics;
using System.Runtime.InteropServices;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay; 

public class Font : NekoObject {
    internal RayFont _font;

    internal Font(RayFont font) {
        _font = font;
        Texture = new Texture {
            _texture = _font.texture
        };
    }
    internal Font() {}

    public bool IsReady => Raylib.IsFontReady(_font);

    public Texture Texture;

    public int BaseSize => _font.baseSize;

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

    public unsafe static Font FromLove2d(Image image, string text) {
        var separateColor = image.GetColorAt(0, 0);
        var height = image.Height;
        var fullWidth = image.Width;
        var texture = image.ToTexture();
        var font = new RayFont {
            baseSize = height,
            texture = texture._texture
        };
        var glyphInfos = new List<GlyphInfo>();
        var glyphRecs = new List<Rectangle>();
        var glyphWidth = 0; 
        for (var i = 1; i <= fullWidth; i++) {
            var isSeparateColor = separateColor.Equals(image.GetColorAt(i, 0));
            if (isSeparateColor || i == fullWidth) {
                var glyphInfo = new GlyphInfo {
                    value = text[font.glyphCount],
                    //offsetX = i,
                };
                font.glyphCount++;
                glyphInfos.Add(glyphInfo);
                glyphRecs.Add(new Rectangle(i-glyphWidth, 0, glyphWidth, height));
                glyphWidth = 0;
            }
            else {
                glyphWidth++;
            }
        }

        var recs = glyphRecs.ToArray();
        var infos = glyphInfos.ToArray();

        font.recs = Utils.TranslateToUnmanaged(recs);
        font.glyphs = Utils.TranslateToUnmanaged(infos);

        return new Font {
                _font = font,
                Texture = texture
            };
    }

    public static Font FromLove2d(string filename, string text) {
        return FromLove2d(Image.Load(filename), text);
    }

    public void Draw(string textString, Vector2? position = null, Vector2? origin = null, float rotation = 0f, float? scale = null, float spacing = 1f, Color? color = null) {
        Raylib.DrawTextPro(
            _font, 
            textString, 
            position??Vector2.Zero, 
            origin??Vector2.Zero, 
            rotation,
            scale??BaseSize,
            spacing,
            color??Raylib.WHITE);
    }

    public override void Dispose() {
        Raylib.UnloadFont(_font);
        Texture = null;
    }
}