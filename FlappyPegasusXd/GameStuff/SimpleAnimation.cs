using NekoLib.Core;
using NekoRay;
using Timer = NekoRay.Timer;

namespace FlappyPegasus.GameStuff; 

public class SimpleAnimation : Behaviour {
    public AnimationFrame[] AnimationFrames;
    public SpriteRenderer2D _spriteRenderer;
    private float _time;
    private int _animationFrameIndex = 0;
    private float _animationTime = 0;
    public AnimationFrame CurrentFrame => AnimationFrames[_animationFrameIndex];
    public int FreezeFrame = -1;
    void UpdateAnimation() {
        _spriteRenderer.Sprite = CurrentFrame.Sprite;
        if (_animationFrameIndex == FreezeFrame) return;
        _animationTime += Timer.DeltaF;
        while (_animationTime >= CurrentFrame.Time) {
            _animationTime -= CurrentFrame.Time;
            _animationFrameIndex++;
            if (_animationFrameIndex >= AnimationFrames.Length) _animationFrameIndex = 0;
        }
    }

    public void RunAnim(int startFrame = 0) {
        _animationFrameIndex = startFrame;
    }
    void Update() {
        UpdateAnimation();
        _time += Timer.DeltaF;
        /*Transform.LocalPosition = 
            new Vector3(MathF.Sin(_time)*128f, MathF.Cos(_time)*128f, 0);*/
    }
}