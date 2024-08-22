using FlappyPegasus.Gui;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using Console = NekoRay.Tools.Console;

namespace FlappyPegasus; 

public class GameOverScene : OverlayScene {
    public override void Initialize() {
        MainGameRoot = new GameObject("Overlay");
        MainGameRoot.ActiveSelf = false;

        var overlay = MainGameRoot.AddChild("Overlay");
        overlay.AddComponent<Canvas>();

        var exit = overlay.AddChild("exit button").AddComponent<Button>();
        exit.Height = 30f;
        exit.Width = 120f;
        exit.OnClick += () => Console.Submit("leave");
        exit.Text = "Leave";

        base.Initialize();
    }
}