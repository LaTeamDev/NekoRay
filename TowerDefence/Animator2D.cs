using NekoLib.Core;
using NekoRay;
using NekoRay.Tools;
using TowerDefence.Objects;
using ZeroElectric.Vinculum;

namespace TowerDefence;

public class Animator2D : Behaviour {
    public string AnimationName;
    public AnimationsDefenition Animation;
    public float AnimationSpeed = 1f;
    public int CurrentAnimationIndex = 0;
    public SpriteRenderer2D SpriteRenderer;
    [ShowInInspector]
    private float _animationTime;
    public Animation? CurrentAnimation {
        get {
            Animation.Animations.TryGetValue(AnimationName, out var value);
            return value;
        }
    }

    public Frame? CurrentFrame {
        get {
            if (CurrentAnimation is null) return null;
            var currentFrameIndex = CurrentAnimation.Frames[CurrentAnimationIndex];
            return Animation.Frames[currentFrameIndex];
        }
    }

    public Rectangle? FrameRectangle {
        get {
            if (CurrentFrame is null) return null;
            return new Rectangle(CurrentFrame.X, CurrentFrame.Y, CurrentFrame.Width, CurrentFrame.Height);
        }
    }

    void Awake() {
        SpriteRenderer = GameObject.GetComponent<SpriteRenderer2D>();
    }
    
    void UpdateAnimation() {
        if (CurrentAnimation is null) return;
        if (CurrentAnimationIndex >= CurrentAnimation.Frames.Count) CurrentAnimationIndex = 0;
        if (AnimationSpeed <= 0f) return;
        _animationTime += Time.DeltaF;
        while (_animationTime >= AnimationSpeed) {
            _animationTime -= AnimationSpeed;
            CurrentAnimationIndex++;
            if (CurrentAnimationIndex >= CurrentAnimation.Frames.Count) CurrentAnimationIndex = 0;
        }
        SpriteRenderer.Sprite = new Sprite(SpriteRenderer.Sprite.Texture, FrameRectangle??new Rectangle());
    }

    void Update() {
        UpdateAnimation();
    }
}