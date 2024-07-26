using System.Numerics;
using FlappyPegasus.GameStuff;
using FlappyPegasus.Gui;
using MessagePack;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using NekoRay.Diagnostics.Model;
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
        camera.Zoom = Raylib.GetRenderHeight() / 288f;
        camera.BackgroundColor = new Color(203, 219, 252, 255);
        var textObject = new GameObject("Text");
        var text = textObject.AddComponent<ShadowedText>();
        text.Font = Data.GetFont("Data/texture/scorefont.png", "0123456789xm.");
        text.ShadowFont = Data.GetFont("Data/texture/scorefont_s.png", "0123456789xm.");
        text.TextString = SaveData.BestScore+"m";
        text.ShadowColor = Raylib.BLACK;
        textObject.Transform.LocalScale = new Vector3(2f, 2f, 0f);
        //cameraObject.AddComponent<ImguiDemoWindow>();
        
        #region Background
        var background = new GameObject("Background");
        background.Transform.Position = new Vector3(-256f, -144f, 0f);
        
        var cloudsB = background.AddChild("CloudsB").AddComponent<ShaderDrawBg>();
        cloudsB.Texture = Data.GetTexture("Data/texture/clouds2.png");
        cloudsB.Transform.LocalPosition = new Vector3(0f, 32f, 0f);

        var cloudsA = background.AddChild("CloudsA").AddComponent<ShaderDrawBg>();
        cloudsA.Texture = Data.GetTexture("Data/texture/clouds1.png");
        
        var groundA = background.AddChild("GroundA").AddComponent<ShaderDrawBg>();
        groundA.Texture = Data.GetTexture("Data/texture/Mountains_A.png");
        groundA.Transform.LocalPosition = new Vector3(0f, 288f - groundA.Texture.Height - 16f, 0f);
        
        var groundB = background.AddChild("GroundB").AddComponent<ShaderDrawBg>();
        groundB.Texture = Data.GetTexture("Data/texture/Mountains_B.png");
        groundB.Transform.LocalPosition = new Vector3(0f, 288f - groundB.Texture.Height, 0f);
        
        var groundC = background.AddChild("GroundC").AddComponent<ShaderDrawBg>();
        groundC.Texture = Data.GetTexture("Data/texture/Tree_A.png");
        groundC.Transform.LocalPosition = new Vector3(0f, 288f - groundC.Texture.Height, 0f);
        
        var groundD = background.AddChild("GroundC").AddComponent<ShaderDrawBg>();
        groundD.Texture = Data.GetTexture("Data/texture/Ground_A.png");
        groundD.Transform.LocalPosition = new Vector3(0f, 288f - groundD.Texture.Height, 0f);
        
        cloudsA.Speed = 2f;
        cloudsB.Speed = 1f;
        groundA.Speed = 0.25f;
        groundB.Speed = 8f;
        groundC.Speed = 32f;
        groundD.Speed = 64f;
        #endregion

        var logo = new GameObject("Logo").AddComponent<SpriteRenderer2D>();
        logo.Sprite = Data.GetSprite("Data/texture/logo.png");
        logo.Transform.Position = new Vector3(240f, -144f + 32f, 0f);
        logo.Origin = new Vector2(1f, 0f);

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
        var bin = MessagePackSerializer.Serialize(new SerializedScene(SceneManager.ActiveScene));
        File.WriteAllBytes("test.bin", bin);
    }
}