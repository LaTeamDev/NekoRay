using FlappyPegasus.Gui;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;

namespace FlappyPegasus; 

public class PauseScene : OverlayScene {
    public override void Initialize() {
        MainGameRoot = new GameObject("Overlay");
        MainGameRoot.ActiveSelf = false;

        var overlay = MainGameRoot.AddChild("Overlay");
        overlay.AddComponent<Canvas>();

        var exit = overlay.AddChild("exit button").AddComponent<Button>();
        exit.Height = 30f;
        exit.Width = 120f;
        exit.OnClick += () => SceneManager.LoadScene(new MenuScene());
        exit.Text = "Leave";
        
        var cont = overlay.AddChild("continue button").AddComponent<Button>();
        cont.Height = 30f;
        cont.Width = 120f;
        cont.OnClick += () => Close();
        cont.Text = "Continue";

        overlay.AddComponent<ButtonLayout>().Calculate();

        base.Initialize();
    }
}