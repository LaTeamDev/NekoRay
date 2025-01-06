using NekoLib.Core;
using NekoLib.Filesystem;
using NekoRay;
using NeuroSama.Gameplay.Dialogue;
using NeuroSama.Gameplay.MiniGame;
using SoLoud;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace NeuroSama.Gameplay.Intro;

public class Intro1Scene : BaseScene {
    private Voice _voice;

    protected virtual void SpawnBackground() {
        var background = new GameObject("background").AddComponent<SpriteRenderer2D>();
        background.Sprite = Data.GetSprite("textures/splash/memory.png");
        background.ProportionallyScaleByHeight(720);
    }

    public override void Initialize() {
        var gameObject = new GameObject("Camera");
        var camera = gameObject.AddComponent<Camera2D>();
        camera.IsMain = true;
        camera.BackgroundColor = Raylib.BLACK;

        SpawnBackground();

        var dg = gameObject.AddChild("Dialogue").AddComponent<DialogueOrchestrator>();
        
        using var stream = Files.GetFile("sounds/music/neuro3.mp3").GetStream();
        var musicStream = WavStream.LoadFromStream(stream);
        _voice = Audio.SoLoud.Play(musicStream);
        _voice.Loop = true;
        
        base.Initialize();
    }

    public override void Dispose() {
        base.Dispose();
        _voice.Stop();
    }

    public virtual void ShowDialogue() {
        DialogueController.Add("Stream", "is on");
        DialogueController.ToggleBox(false);
        DialogueController.Fade(0f);
        DialogueController.Add("Neuro", "Vedal.");
        DialogueController.Add("Vedal", "huh?");
        DialogueController.Add("Neuro", "Did i ever had...");
        DialogueController.Add("Neuro", "... a childhood?");
        DialogueController.RemoveImage(DialoguePosition.Center);
        DialogueController.ChangeScene(new SplashScene(new Intro2Scene(), "thepast"));
    }

    private bool _dialogueShown = false;
    public override void Update() {
        base.Update();
        if (!_dialogueShown) {
            _dialogueShown = true;
            ShowDialogue();
        }
    }

    public override void OnWindowResize() {
        base.OnWindowResize();
        ((Camera2D) (BaseCamera.Main)).Zoom = 1280 / BaseCamera.Main.RenderWidth;
    }
}