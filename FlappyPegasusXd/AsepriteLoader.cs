using System.Text.Json;
using NekoRay;
using System.Numerics;
using System.Text.Json.Serialization;
using ZeroElectric.Vinculum;

namespace FlappyPegasus; 

public static class AsepriteLoader {
    public static AnimationFrame[] Load(string path) {
        //if (!File.Exists(path)) return Array.Empty<AnimationFrame>();

        var text = Raylib.LoadFileText(path);//File.ReadAllText(path);
        Console.WriteLine(path);
        using var obj = JsonDocument.Parse(text);

        var filename = obj.RootElement.GetProperty("meta").GetProperty("image").GetString()??Path.GetFileNameWithoutExtension(path)+".png";
        var texture = Data.GetTexture(Path.Combine(Path.GetDirectoryName(path)??"", filename));

        var frames = obj.RootElement.GetProperty("frames");

        var sprites = new List<AnimationFrame>();
        foreach (var frameProp in frames.EnumerateObject()) {
            var frame = frameProp.Value;
            var rect = frame.GetProperty("frame");
            var sprite = new Sprite(texture, new Rectangle(
                (float)rect.GetProperty("x").GetDecimal(), 
                (float)rect.GetProperty("y").GetDecimal(), 
                (float)rect.GetProperty("w").GetDecimal(), 
                (float)rect.GetProperty("h").GetDecimal()));
            sprites.Add(new AnimationFrame(sprite, (float)frame.GetProperty("duration").GetDecimal()/1000));
        }

        return sprites.ToArray();
    }
}