namespace HotlineSPonyami; 

public class Animation {
    public AnimationFrame[] AnimationFrames;
    private float _time;
    private int _animationFrameIndex = 0;
    private float _animationTime = 0;
    public AnimationFrame CurrentFrame => AnimationFrames[_animationFrameIndex];
    public int FreezeFrame = -1;

    public Animation(AnimationFrame[] animationFrames) {
        AnimationFrames = animationFrames;
    }
    
   void UpdateAnimation() {
        if (_animationFrameIndex == FreezeFrame) return;
        _animationTime += NekoRay.Time.DeltaF;
        while (_animationTime >= CurrentFrame.Duration) {
            _animationTime -= CurrentFrame.Duration;
            _animationFrameIndex++;
            if (_animationFrameIndex >= AnimationFrames.Length) _animationFrameIndex = 0;
        }
    }
   
   public void Update() {
        UpdateAnimation();
        _time += NekoRay.Time.DeltaF;
        /*Transform.LocalPosition = 
            new Vector3(MathF.Sin(_time)*128f, MathF.Cos(_time)*128f, 0);*/
    }
}