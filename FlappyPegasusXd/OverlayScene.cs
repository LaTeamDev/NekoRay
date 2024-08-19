using System.Numerics;
using NekoLib.Core;
using NekoLib.Scenes;
using ZeroElectric.Vinculum;
using NekoRay;

namespace FlappyPegasus; 

public abstract class OverlayScene : BaseScene {

    private IScene _prevScene;
    protected GameObject MainGameRoot;
    public bool Active => SceneManager.ActiveScene == this;
    
    public void Open() {
        if (Active) return;
        _prevScene = SceneManager.ActiveScene;
        SceneManager.SetSceneActive(this);
        MainGameRoot.ActiveSelf = true;
    }

    public void Close() {
        if (!Active) return;
        SceneManager.SetSceneActive(_prevScene);
        MainGameRoot.ActiveSelf = false;
    }

    public void Toggle() {
        if (!Active) Open();
        else Close();
    }

    public override void Draw() {
        if (Active) {
            var texture = BaseCamera.Main.RenderTexture.Texture;
            var rect = new Rectangle(0, 0, texture.Width, -texture.Height);
            var rectDest = new Rectangle(0, 0, texture.Width, texture.Height);
            texture.Draw(rect, rectDest, Vector2.Zero, 0f, new Color(200, 200, 200, 255));
        }
        
        base.Draw();
    }
}