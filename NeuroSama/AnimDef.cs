using NekoLib.Filesystem;
using NekoRay;
using Tomlyn;

namespace NeuroSama;

public class AnimDef {
    public float AnimationSpeed { get; set; }
    public List<string> Frames { get; set; }
    public List<Sprite> FramesSprites = new();

    public static AnimDef FromFile(string path) {
        var file = Toml.ToModel<AnimDef>(Files.GetFile(path).Read());
        foreach (var frame in file.Frames) {
            file.FramesSprites.Add(Data.GetSprite(Path.Join(Path.GetDirectoryName(path), frame)));
        }
        return file;
    }
}