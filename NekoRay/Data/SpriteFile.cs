using System.Drawing;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using NekoLib.Filesystem;

namespace NekoRay.Data;

public class SpriteFile {
    public string Texture { get; set; }
    public RectangleF Bounds { get; set; }
    public Vector2 Origin { get; set; }
    public float PixelSize { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter<TextureFilter>))]
    public TextureFilter ImageFilter { get; set; } = TextureFilter.Bilinear;
    public Dictionary<string, Vector2> AttachmentPoints { get; set; }

    public static SpriteFile? Load(string path) {
        using var stream = Files.GetFile(path).GetStream();
        return JsonSerializer.Deserialize<SpriteFile>(stream, SourceGenerationContext.Default.SpriteFile);
    }
}