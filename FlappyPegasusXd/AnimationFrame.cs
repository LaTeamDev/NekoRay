using NekoRay;

namespace FlappyPegasus; 

public class AnimationFrame {
    public Sprite Sprite;
    public float Time;

    
    public AnimationFrame(Sprite sprite, float time = 0.1f) {
        if (time <= 0f) throw new ArgumentOutOfRangeException(nameof(time));
        Sprite = sprite;
        Time = time;
    }
}