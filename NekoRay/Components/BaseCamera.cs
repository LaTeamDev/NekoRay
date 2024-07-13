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
            }
            if (Main is not null) {
                Main._isMain = false;
            }
            Main = this;
        }
    }

    public RenderTexture RenderTexture = RenderTexture.Load(Raylib.GetRenderWidth(), Raylib.GetRenderHeight());
}