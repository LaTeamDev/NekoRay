using NekoLib.Core;
using NekoLib.Extra;
using NekoRay;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace NekoRay.Template;

public class YourScene : Scene {
    public override void Initialize() {
        var gameObject = new GameObject("Camera");
        var camera = gameObject.AddComponent<Camera2D>();
        camera.BackgroundColor = Raylib.RAYWHITE;
        camera.IsMain = true;

        var text = new GameObject("Text").AddComponent<YourBehaviour>();
    }
}