using System.Text.Json;
using NekoRay;
using Serilog;
using ZeroElectric.Vinculum;

namespace HotlineSPonyami;

public class AnimationFrame {
    public float Duration;
    public Sprite Sprite;

    public AnimationFrame(Sprite sprite, float duration = 0.1f) {
        Sprite = sprite;
        Duration = duration;
    }
}

public static class AsepriteLoader {
    public static AnimationManifest? Parse(string path) {
        var text = Raylib.LoadFileText(path);
        var stuff = JsonSerializer.Deserialize<AnimationManifest>(text);
        return stuff;
    }

    public static AnimationFrame[] Load(string path) {
        var manifest = Parse(path);
        if (manifest is null) {
            Log.Error("Faile to load animation from file {Filepath}", path);
        }
        return Load(manifest, Path.GetDirectoryName(path)??"");
    }
    public static AnimationFrame[] Load(AnimationManifest animationManifest, string basepath = "") {
        var frames = new List<AnimationFrame>();
        var imageLocation = Path.Combine(basepath, animationManifest.meta.image);
        foreach (var frame in animationManifest.frames) {
            frames.Add(new AnimationFrame(
                new(
                    Data.GetTexture(imageLocation), 
                    new Rectangle(frame.frame.x, frame.frame.y, frame.frame.w, frame.frame.h)
                    ), 
                frame.duration/1000f)
            );
        }

        return frames.ToArray();
    }
}