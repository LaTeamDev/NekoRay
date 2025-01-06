using System.Numerics;
using NekoLib.Core;
using NekoLib.Filesystem;
using NekoLib.Scenes;
using NekoRay;
using NekoRay.Tools;
using NeuroSama.Gameplay.Intro;
using NeuroSama.UI;
using SoLoud;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace NeuroSama.Gameplay.MainMenu;

public class MenuScene : BaseScene {
    public override void Initialize() {
        var gameObject = new GameObject("Camera");
        var camera = gameObject.AddComponent<Camera2D>();
        camera.IsMain = true;
        camera.BackgroundColor = Raylib.BLACK;

        var bg = new GameObject("Background")
            .AddComponent<SpriteRenderer2D>().GameObject
            .AddComponent<MenuBackgroundAnimation>();
        bg.Sprites.Add(Data.GetSprite("textures/menu_alt.png"));
        bg.Sprites.Add(Data.GetSprite("textures/menu.png"));
        var gradient = new GameObject("Gradient") 
            .AddComponent<SpriteRenderer2D>();
        gradient.Sprite = Data.GetSprite("textures/gradient.png");
        gradient.Width = -475f;
        gradient.Height = 720f;
        gradient.Sprite.Texture.Filter = TextureFilter.TEXTURE_FILTER_TRILINEAR;
        gradient.Sprite.Texture.Wrap = TextureWrap.TEXTURE_WRAP_CLAMP;

        var playButton = CreateText("Play Button", "play")
            .GameObject.AddComponent<Button>();

        var optionsButton = CreateText("Options Button", "options")
            .GameObject.AddComponent<Button>();

        var exitButton = CreateText("Exit Button", "exit")
            .GameObject.AddComponent<Button>();
        
        playButton.GameObject.Transform.Position = new Vector3(512f, 0f, 0f);
        optionsButton.GameObject.Transform.Position = new Vector3(454f, 128f, 0f);
        exitButton.GameObject.Transform.Position = new Vector3(-480f, 314f, 0f);
        exitButton.GameObject.AddComponent<QuitButtonAction>();
        playButton.GameObject.AddComponent<PlayButtonAction>();
        optionsButton.GameObject.Active = false;
        
        using var stream = Files.GetFile("sounds/music/neuro1.mp3").GetStream();
        var musicStream = WavStream.LoadFromStream(stream);
        _voice = Audio.SoLoud.Play(musicStream);
        _voice.Loop = true; 
        
        var logo = new GameObject("Logo") 
            .AddComponent<SpriteRenderer2D>();
        logo.Sprite = Data.GetSprite("textures/logo.png");
        logo.Transform.Position = new Vector3(0, -243f, 0f);
        logo.Transform.LocalScale = new Vector3(2f);
        
        base.Initialize();
    }
    private Voice _voice;

    public override void Dispose() {
        _voice.Stop();
        base.Dispose();
    }

    private Text CreateText(string name, string textStr) {
        var text = new GameObject(name)
            .AddComponent<TextMenuAnimation>()
            .GameObject.AddComponent<Text>();
        text.TextString = textStr;
        text.Color = Raylib.RED;
        text.Font = Data.GetFont("fonts/menu_font.ttf");
        text.Origin = Vector2.One / 2;
        return text;
    }

    [ConCommand("newgame")]
    public static void Play() {
        SceneManager.LoadScene(new Intro1Scene());
    }
    
    [ConCommand("options")]
    public static void Options() {
        throw new NotImplementedException();
    }
    
    [ConCommand("menu")]
    public static void Leave() {
        SceneManager.LoadScene(new MenuScene());
    }
}