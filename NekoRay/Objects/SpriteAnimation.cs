using System.Numerics;
using NekoLib.Filesystem;
using NekoRay.Data;

namespace NekoRay;

public class SpriteAnimation : NekoObject, IAsset {
    public class Frame(Sprite sprite) {
        public Sprite Sprite = sprite;
        public float Time = 0.1f;
        public Vector2 Scale = Vector2.One;
        public Vector2 Offset = Vector2.Zero;
        public bool Interpolation = false;
    }

    public List<Frame> Frames = new();
    public float Speed = 1;
    public float SpeedModifier = 1;
    
    public float OverallSpeed => Speed*SpeedModifier;
    
    public string Path { get; private set; } = "";

    public static SpriteAnimation LoadUncached(string path) {
        var anim = new SpriteAnimation {
            Path = path,
        };
        anim.Reload();
        return anim;
    }

    public static SpriteAnimation Load(string path) {
        path = path.Replace('\\', '/');
        if (AssetCache.TryGet<SpriteAnimation>(path, out var sprite)) return sprite;
        if (!Files.FileExists(path)) throw new FileNotFoundException();
        sprite = LoadUncached(path);
        return AssetCache.Add(sprite);
    }
    
    public void Reload() {
        var animFile = SpriteAnimationFile.Load(Path);
        Name = animFile.Name;
        Speed = animFile.Speed;
        Frames.Clear();
        foreach (var frameFile in animFile.Frames)
            Frames.Add(new Frame(Sprite.Load(frameFile.Sprite)) {
                Interpolation = frameFile.Interpolation,
                Offset = frameFile.Offset,
                Scale = frameFile.Scale,
                Time = frameFile.Time
            });
    }

    public override void Dispose() {
        if (Path != null) AssetCache.Remove(this);
    }
}
