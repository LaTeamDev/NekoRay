using System.Numerics;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using NekoRay.Easings;
using NeuroSama.UI;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;
using Font = NekoRay.Font;

namespace NeuroSama.Gameplay.Dialogue;

public class DialogueOrchestrator : Behaviour, IObserver<DialogueEvent> {
    public Rect TopBox;
    public Rect BottomBox;
    public Rect Fader;
    public Text CurrentText;
    public Text PrevText;
    public Text NextText;
    public SpriteRenderer2D SpriteLeft;
    public SpriteRenderer2D SpriteCenter;
    public SpriteRenderer2D SpriteRight;
    
    public Queue<DialogueEvent> Queue = new();
    private IDisposable _trackerHandle;

    void Awake() {
        var fader = GameObject.AddChild("Fader").AddComponent<Rect>();
        SpriteLeft = GameObject.AddChild("Sprite Left").AddComponent<SpriteRenderer2D>();
        SpriteCenter = GameObject.AddChild("Sprite Center").AddComponent<SpriteRenderer2D>();
        SpriteRight = GameObject.AddChild("Sprite Right").AddComponent<SpriteRenderer2D>();
        
        var topBox = GameObject.AddChild("Top Box").AddComponent<Rect>();
        var bottomBox = GameObject.AddChild("Bottom Box").AddComponent<Rect>();
        Transform.LocalPosition = new Vector3(-640, -360, 0);
        fader.Width = 1280;
        fader.Height = 720;
        fader.Color = Raylib.BLACK.Fade(0.25f);
        topBox.Width = 1280;
        topBox.Height = 128;
        topBox.Color = Raylib.BLACK;
        bottomBox.Width = 1280;
        bottomBox.Height = 128;
        bottomBox.Color = Raylib.BLACK;
        bottomBox.Origin = new Vector2(0, 1f);
        bottomBox.Transform.LocalPosition = new Vector3(0, 720, 0);
        var text = bottomBox.GameObject.AddChild("Text").AddComponent<Text>();
        var font = Font.Load("fonts/dialogue_font.ttf", 16, 255); //FIXME: this is uncached 
        text.Font = font;
        text.TextString = "(Press: space, lmb, rmb, e or enter to cont.)";
        text.Origin = new Vector2(0.5f, 0.5f);
        text.Transform.LocalPosition = new Vector3(640, -64, 0);
        text.Transform.LocalScale = new Vector3(2f);
        
        TopBox = topBox;
        BottomBox = bottomBox;
        Fader = fader;
        CurrentText = text;
        
        NextText = bottomBox.GameObject.AddChild("Next Text").AddComponent<Text>();
        NextText.Font = font;
        NextText.TextString = "";
        NextText.Origin = new Vector2(0.5f, 0.5f);
        NextText.Transform.LocalPosition = new Vector3(640, -32, 0);
        NextText.Transform.LocalScale = new Vector3(2f);
        NextText.Color = Raylib.WHITE.Fade(0f);
        
        PrevText = bottomBox.GameObject.AddChild("Previous Text").AddComponent<Text>();
        PrevText.Font = font;
        PrevText.TextString = "";
        PrevText.Origin = new Vector2(0.5f, 0.5f);
        PrevText.Transform.LocalPosition = new Vector3(640, -96, 0);
        PrevText.Color = Raylib.WHITE.Fade(0f);
        
        SpriteLeft.Sprite = Data.GetSprite("textures/dialogues/neuro neutral.png");
        SpriteCenter.Sprite = SpriteLeft.Sprite;
        SpriteRight.Sprite = SpriteLeft.Sprite;
        SpriteLeft.Transform.LocalPosition = new Vector3(256, 467, 0);
        SpriteCenter.Transform.LocalPosition = new Vector3(640, 467, 0);
        SpriteRight.Transform.LocalPosition = new Vector3(1024, 467, 0);
        SpriteLeft.Color = SpriteRight.Color = SpriteCenter.Color = Raylib.WHITE.Fade(0f);

        _trackerHandle = DialogueController.DialogueObservable.Subscribe(this);
    }

    public void OnCompleted() { }

    public void OnError(Exception error) { }

    public void OnNext(DialogueEvent? value) {
        if (value is null) {
            OnNext();
            return;
        }
        if (value.GetType() != typeof(DialogueNext)) {
            Queue.Enqueue(value);
            return;
        }
        OnNext();
    }

    public void OnNext() {
        PrevEvent = Queue.Dequeue();
        Next();
        if (NextEvent?.Skip??false) OnNext(); 
    }

    private DialogueEvent? PrevEvent;

    private DialogueEvent? NextEvent {
        get {
            Queue.TryPeek(out var val);
            return val;
        }
    }

    public override void Dispose() {
        base.Dispose();
        _trackerHandle.Dispose();
    }

    void Update() {
        switch (Queue.Count) {
            case > 0 when !_shown:
                _shown = true;
                _hidden = false;
                Show();
                break;
            case <= 0 when !_hidden:
                _shown = false;
                _hidden = true;
                Hide();
                break;
        }

        if (!IsInDialogue) return;
        if (Input.IsPressed("next")||Input.IsPressed("attack")) OnNext();
    }

    private bool _shown;
    private bool _hidden;

    public static bool IsInDialogue { get; private set; }

    public IEasing Easing = new EaseInOutCubic();
    public float Time = 0.3f;
    public void Show() {
        IsInDialogue = true;
        if (NextEvent is DialogueEntry dialog) {
            CurrentText.TextString = $"{dialog.Name}: {dialog.Text}";
        }
        Next();
        
        Ease.To(() => TopBox.Transform.LocalPosition.Y, 
            value => TopBox.Transform.LocalPosition = TopBox.Transform.LocalPosition with {Y =value}, 
            Time,
            0).SetEasing(Easing);
        Ease.To(() => BottomBox.Transform.LocalPosition.Y, 
            value => BottomBox.Transform.LocalPosition = BottomBox.Transform.LocalPosition with {Y = value}, 
            Time,
            720).SetEasing(Easing);
        Ease.To(() => CurrentText.Transform.LocalPosition.Y, 
            value => CurrentText.Transform.LocalPosition = CurrentText.Transform.LocalPosition with {Y = value}, 
            Time,
            -64).SetEasing(Easing);
        Ease.To(() => (float)Fader.Color.a,
            value => Fader.Color = Fader.Color with {a = (byte)value}, 
            Time,
            69).SetEasing(Easing);
        Ease.To(() => (float)CurrentText.Color.a,
            value => CurrentText.Color = CurrentText.Color with {a = (byte)value}, 
            Time,
            255).SetEasing(Easing);
    }

    public void Next() {
        if (Queue.Count <= 0) return;
        if (NextEvent is DialogueEntry dialog) ShowNextText(dialog);
        if (NextEvent is DialogueShowSprite showSprite) ShowSprite(showSprite);
        if (NextEvent is DialogueHideSprite hideSprite) HideSprite(hideSprite);
        if (NextEvent is DialoguePlaySound playSound) Audio.SoLoud.Play(playSound.AudioSource);
        if (NextEvent is DialogueSetFadeBg fadeBg) Ease.To(() => Fader.Color.a,
            value => Fader.Color = Fader.Color with {a = (byte)value}, 
            Time,
            255f*fadeBg.Ammount).SetEasing(Easing);
        if (NextEvent is DialogueToggleUpper upper) {
            if (upper.State) {
                Ease.To(() => TopBox.Transform.LocalPosition.Y, 
                    value => TopBox.Transform.LocalPosition = TopBox.Transform.LocalPosition with {Y =value}, 
                    Time,
                    0).SetEasing(Easing);
            }
            else {
                Ease.To(() => TopBox.Transform.LocalPosition.Y, 
                    value => TopBox.Transform.LocalPosition = TopBox.Transform.LocalPosition with {Y =value}, 
                    Time,
                    -128).SetEasing(Easing);
            }
        }
        if (NextEvent is DialogueChangeScene changeScene) {
            Ease.CancelAll();
            SceneManager.LoadScene(changeScene.Scene);
        };
    }

    public void ShowNextText(DialogueEntry dialog) {
        NextText.TextString = $"{dialog.Name}: {dialog.Text}";
        Ease.To(() => CurrentText.Transform.LocalPosition.Y, 
            value => CurrentText.Transform.LocalPosition = CurrentText.Transform.LocalPosition with {Y = value}, 
            Time,
            -96).SetEasing(Easing).After(
            ()=>CurrentText.Transform.LocalPosition = CurrentText.Transform.LocalPosition with {Y = -64}
        );;
        Ease.To(() => (float)CurrentText.Color.a,
            value => CurrentText.Color = CurrentText.Color with {a = (byte)value}, 
            Time,
            96).SetEasing(Easing).After(
            ()=>CurrentText.Color = CurrentText.Color with {a = 255}
        );
        Ease.To(() => CurrentText.Transform.LocalScale.X, 
            value => CurrentText.Transform.LocalScale = new(value), 
            Time,
            1).SetEasing(Easing).After(
            ()=>CurrentText.Transform.LocalScale = new(2)
        );
        Ease.To(() => NextText.Transform.LocalPosition.Y, 
            value => NextText.Transform.LocalPosition = NextText.Transform.LocalPosition with {Y = value}, 
            Time,
            -64).SetEasing(Easing).After(
            ()=>NextText.Transform.LocalPosition = NextText.Transform.LocalPosition with {Y = -32}
        );
        Ease.To(() => (float)PrevText.Color.a,
            value => PrevText.Color = PrevText.Color with {a = (byte)value}, 
            Time,
            0).SetEasing(Easing).After(
            ()=> {
                PrevText.Color = PrevText.Color with {a = 96};
            });
        
        Ease.To(() => (float)NextText.Color.a,
            value => NextText.Color = NextText.Color with {a = (byte)value}, 
            Time,
            255).SetEasing(Easing).After(
            ()=> {
                NextText.Color = NextText.Color with {a = 0};
                PrevText.TextString = CurrentText.TextString;
                CurrentText.TextString = NextText.TextString;
            });
    }

    public void ShowSprite(DialogueShowSprite showSprite) {
        var spritePos = showSprite.Position;
        var sprite = Data.GetSprite($"textures/dialogues/{showSprite.Name} {showSprite.Emotion}.png");
        var renderer = spritePos switch {
            DialoguePosition.Left => SpriteLeft,
            DialoguePosition.Center => SpriteCenter,
            DialoguePosition.Right => SpriteRight,
            _ => throw new ArgumentOutOfRangeException()
        };
        var direction = spritePos switch {
            DialoguePosition.Left => AnimationDirection.Right,
            DialoguePosition.Center => AnimationDirection.Down,
            DialoguePosition.Right => AnimationDirection.Left,
            _ => throw new ArgumentOutOfRangeException()
        };
        renderer.Sprite = sprite;
        renderer.ProportionallyScaleByWidth(450);
        SpriteFade(renderer, direction, true);
    }
    
    private void HideSprite(DialogueHideSprite hideSprite) {
        var spritePos = hideSprite.Position;
        var renderer = spritePos switch {
            DialoguePosition.Left => SpriteLeft,
            DialoguePosition.Center => SpriteCenter,
            DialoguePosition.Right => SpriteRight,
            _ => throw new ArgumentOutOfRangeException()
        };
        var direction = spritePos switch {
            DialoguePosition.Left => AnimationDirection.Left,
            DialoguePosition.Center => AnimationDirection.Up,
            DialoguePosition.Right => AnimationDirection.Right,
            _ => throw new ArgumentOutOfRangeException()
        };
        SpriteFade(renderer, direction, false);
    }
    
    private enum AnimationDirection {
        Up,
        Down,
        Left,
        Right
    }

    public float SpriteFadeOffset = 128f;

    private void SpriteFade(SpriteRenderer2D renderer, AnimationDirection direction, bool show) {
        if (renderer.GameObject.Active == show) return;
        switch (direction) {
            case AnimationDirection.Up:
                Ease.To(() => renderer.Transform.LocalPosition.Y,
                    value => renderer.Transform.LocalPosition = renderer.Transform.LocalPosition with {Y = value},
                    Time,
                    renderer.Transform.LocalPosition.Y + SpriteFadeOffset).SetEasing(Easing);
                break;
            case AnimationDirection.Down:
                Ease.To(() => renderer.Transform.LocalPosition.Y,
                    value => renderer.Transform.LocalPosition = renderer.Transform.LocalPosition with {Y = value},
                    Time,
                    renderer.Transform.LocalPosition.Y - SpriteFadeOffset).SetEasing(Easing);
                break;
            case AnimationDirection.Left:
                Ease.To(() => renderer.Transform.LocalPosition.X,
                    value => renderer.Transform.LocalPosition = renderer.Transform.LocalPosition with {X = value},
                    Time,
                    renderer.Transform.LocalPosition.X - SpriteFadeOffset).SetEasing(Easing);
                break;
            case AnimationDirection.Right:
                Ease.To(() => renderer.Transform.LocalPosition.X,
                    value => renderer.Transform.LocalPosition = renderer.Transform.LocalPosition with {X = value},
                    Time,
                    renderer.Transform.LocalPosition.X + SpriteFadeOffset).SetEasing(Easing);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
        if (show) renderer.GameObject.Active = true;
        Ease.To(() => (float) renderer.Color.a,
            value => renderer.Color = renderer.Color with {a = (byte) value},
            Time,
            show ? 255 : 0).SetEasing(Easing).After(() => {
            if (!show) renderer.GameObject.Active = false;
        });
}
    
    public void Hide() {
        IsInDialogue = false;
        NextText.TextString = "";
        Ease.To(() => TopBox.Transform.LocalPosition.Y, 
            value => TopBox.Transform.LocalPosition = TopBox.Transform.LocalPosition with {Y =value}, 
            Time,
            -128).SetEasing(Easing);
        Ease.To(() => BottomBox.Transform.LocalPosition.Y, 
            value => BottomBox.Transform.LocalPosition = BottomBox.Transform.LocalPosition with {Y = value}, 
            Time,
            848).SetEasing(Easing);
        Ease.To(() => CurrentText.Transform.LocalPosition.Y, 
            value => CurrentText.Transform.LocalPosition = CurrentText.Transform.LocalPosition with {Y = value}, 
            Time,
            -192).SetEasing(Easing);
        Ease.To(() => (float)Fader.Color.a,
            value => Fader.Color = Fader.Color with {a = (byte)value}, 
            Time,
            0).SetEasing(Easing);
        Ease.To(() => (float)CurrentText.Color.a,
            value => CurrentText.Color = CurrentText.Color with {a = (byte)value}, 
            Time,
            0).SetEasing(Easing);
        SpriteFade(SpriteRight, AnimationDirection.Right, false);
        SpriteFade(SpriteLeft, AnimationDirection.Left, false);
        SpriteFade(SpriteCenter, AnimationDirection.Up, false);
    }
}