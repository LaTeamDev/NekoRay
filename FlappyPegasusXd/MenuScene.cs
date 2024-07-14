using System.Numerics;
using FlappyPegasus.Gui;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;
using Font = NekoRay.Font;
using Music = NekoRay.Music;

namespace FlappyPegasus; 

public class MenuScene : BaseScene {
    public override void Initialize() {
        var cameraObject = new GameObject("Camera");
        var camera = cameraObject.AddComponent<Camera2D>();
        camera.BackgroundColor = Raylib.BLUE;
        camera.IsMain = true;
        var textObject = new GameObject("Text");
        var text = textObject.AddComponent<ShadowedText>();
        text.Font = Font.FromLove2d("Data/texture/scorefont.png", "0123456789xm");
        text.ShadowFont = Font.FromLove2d("Data/texture/scorefont_s.png", "0123456789xm");
        text.TextString = "280m";
        text.ShadowColor = Raylib.BLACK;
        textObject.Transform.LocalScale = new Vector3(2f, 2f, 0f);

        var canvas = new GameObject("Canvas");
        canvas.AddComponent<Canvas>();
        
        var gameButtons = canvas.AddChild("Buttons");
        var layout = gameButtons.AddComponent<ButtonLayout>();
        layout.Transform.LocalPosition = new Vector3(120, 240, 0);
            
        var guiPlay = gameButtons.AddChild("PlayButton").AddComponent<Button>();
        guiPlay.Text = "Play";
        guiPlay.OnClick += () => SceneManager.LoadScene(new GameScene());
        guiPlay.Width = 120f;
        guiPlay.Height = 30f;
        
        var guiShop = gameButtons.AddChild("ShopButton").AddComponent<Button>();
        guiShop.Text = "Shop";
        guiShop.Disabled = true;
        guiShop.Width = 120f;
        guiShop.Height = 30f;
        
        var guiExit = gameButtons.AddChild("ExitButton").AddComponent<Button>();
        guiExit.Text = "Exit";
        guiExit.OnClick += Program.Quit;
        guiExit.Width = 120f;
        guiExit.Height = 30f;

        layout.Calculate();
        
        var audio = new GameObject("Level Music").AddComponent<AudioPlayer>();
        audio.AudioClip = Music.Load("Data/TownTheme.mp3");
        audio.Loop = true;
        audio.Play();
        
        base.Initialize();
    }
}