using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using NekoRay.Easings;
using NeuroSama.Gameplay.MainMenu;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace NeuroSama;

public class SplashScene : BaseScene {
    public Queue<string> Splashes = new();

    public SplashScene(IScene next, params string[] splashes) {
        foreach (var splash in splashes) {
            ShowSplash(splash);
        }

        NextScene = next;
    }
    
    public override void Initialize() {
        var gameObject = new GameObject("Camera");
        var camera = gameObject.AddComponent<Camera2D>();
        camera.IsMain = true;
        base.Initialize();
    }

    public IScene NextScene;
    
    private Ease _ease;

    public void ShowSplash(string name) {
        Splashes.Enqueue(name);
    }

    public override void Update() {
        base.Update();
        if (_showingSplash) {
            if (Input.IsPressed("next")) _ease.Stop();
            return;
        }
        if (Splashes.Count > 0) {
            CreateSplash(Splashes.Dequeue());
            return;
        } 
        SceneManager.LoadScene(NextScene);
    }

    private IEasing Easing = new EaseLinear();
    private float Time = 1f;
    private float TimeHold = 5f;
    private bool _showingSplash = false;

    private void CreateSplash(string name) {
        _showingSplash = true;
        var splash = new GameObject(name).AddComponent<SpriteRenderer2D>();
        splash.Sprite = Data.GetSprite(Path.Join("textures", "splash", name + ".png"));
        splash.Width = 1280;
        splash.Height = 720;
        splash.Color = Raylib.WHITE.Fade(0f);
        _ease = Ease.To(() => splash.Color.a,
            value => splash.Color = splash.Color with {a = (byte)value}, 
            Time,
            255).SetEasing(Easing).After(
            ()=> {
                _ease = Ease.To(() => splash.Color.a,
                    value => splash.Color = splash.Color with {a = (byte) value},
                    TimeHold,
                    255).SetEasing(Easing).After(
                    () => {
                        _ease = Ease.To(() => splash.Color.a,
                            value => splash.Color = splash.Color with {a = (byte) value},
                            Time,
                            0).SetEasing(Easing).After(
                            () => {
                                _showingSplash = false;
                            });
                    });
            });
    }
}