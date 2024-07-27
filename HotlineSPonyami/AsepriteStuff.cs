using System.Text.Json.Serialization;

namespace HotlineSPonyami; 

public class AsepriteAnimationFrame {
    public class Rect {
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h{ get; set; }
    }
    public class Size {
        public int w { get; set; }
        public int h { get; set; }
    }

    public string filename { get; set; }
    public Rect frame { get; set; }
    public bool rotated { get; set; }
    public bool trimmed { get; set; }
    public Rect spriteSourceSize { get; set; }
    public Rect sourceSize { get; set; }
    public float duration { get; set; }
    
}

public class AnimationManifest {
    public class Meta {
        public string app { get; set; }
        public string version { get; set; }
        public string image { get; set; }
        public string format { get; set; }
        public AsepriteAnimationFrame.Rect size { get; set; }
    }
    public AsepriteAnimationFrame[] frames { get; set; }
    public Meta meta { get; set; }
}