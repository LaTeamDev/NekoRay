using System.Numerics;
using NekoLib.Core;
using ZeroElectric.Vinculum;

namespace NekoRay; 

public abstract class BaseCamera : Behaviour {
    public static BaseCamera? CurrentCamera { get; protected set; }
    public static BaseCamera? Main { get; private set; }
    internal bool _isMain;
    public bool IsMain {
        get => _isMain;
        set {
            _isMain = value;
            if (!_isMain) {
                if (Main == this) Main = null;
                return;
            }
            if (Main is not null) {
                Main._isMain = false;
            }
            Main = this;
        }
    }

    public RenderTexture RenderTexture = RenderTexture.Load(Raylib.GetRenderWidth(), Raylib.GetRenderHeight());

    public abstract Vector2 WorldToScreen(Vector3 position);
    public abstract Vector3 ScreenToWorld(Vector2 position);

    public override void Dispose() {
        base.Dispose();
        IsMain = false;
    }
}