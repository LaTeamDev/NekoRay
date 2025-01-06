using NekoLib.Core;
using NekoRay;
using NekoRay.Tools;

namespace NeuroSama.Gameplay.MainMenu;

public class MenuBackgroundAnimation  : Behaviour {
    private SpriteRenderer2D _renderer;
    public List<Sprite> Sprites = new();
    private int _frame;
    private float _frametime;
    [Range(0.01f, 1f)]
    public float AnimationSpeed = 0.1f;
    
    void Awake() {
        _renderer = GameObject.GetComponent<SpriteRenderer2D>();
    }

    void Update() {
        if (Sprites.Count < 1) return;
        _frametime += Time.DeltaF;
        while (_frametime > AnimationSpeed) {
            _frametime -= AnimationSpeed;
            _frame++;
            if (_frame >= Sprites.Count) _frame = 0;
        }

        _renderer.Sprite = Sprites[_frame];
        _renderer.Width = 1280;
        _renderer.Height = 720;
    }
}