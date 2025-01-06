using System.Numerics;
using NekoLib.Core;
using NekoRay;
using NekoRay.Tools;

namespace NeuroSama.Gameplay.MainMenu;

public class TextMenuAnimation : Behaviour {
    public Text OriginalText; 
    public Text ShadowText;
    
    public Vector2 ShadowTextAnchor;
    
    [Range(32f)]
    public Vector2 MaxDistance = Vector2.One*16f;

    [Range(16f)] 
    public Vector2 JitterAmmount = Vector2.One*16f;

    private float _frameTime = 9999999999999999f;

    [Range(1f)] 
    public float AnimationSpeed = 0.1f;

    private Random _random = new();
    
    
    void Awake() {
        OriginalText = GameObject.GetComponent<Text>();
        ShadowText = GameObject.AddChild("Shadow Text").AddComponent<Text>();
        ShadowText.Font = OriginalText.Font;
        ShadowText.Color = OriginalText.Color.Fade(0.5f);
        ShadowTextAnchor = new Vector2(
            MaxDistance.X * (_random.NextSingle() * 2 - 1),
            MaxDistance.Y * (_random.NextSingle() * 2 - 1));
        ShadowText.TextString = OriginalText.TextString;
        ShadowText.Origin = Vector2.One / 2;
    }

    void Update() {
        _frameTime += Time.DeltaF;
        if (AnimationSpeed < _frameTime) {
            _frameTime = 0;
            var jitter = new Vector2(
                JitterAmmount.X * (_random.NextSingle() * 2 - 1),
                JitterAmmount.Y * (_random.NextSingle() * 2 - 1));
            ShadowText.Transform.LocalPosition = new Vector3(ShadowTextAnchor + jitter, 0f);
        }
    }
}