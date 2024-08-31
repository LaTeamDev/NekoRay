using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using NekoRay.Tools;

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
    
    [HideInInspector]
    public RenderTexture? RenderTexture;

    public BaseCamera() {
        RecreateRenderTexture();
    }
    
    public void RecreateRenderTexture() {
        if (RenderTexture is not null) RenderTexture.Dispose(); 
        RenderTexture = RenderTexture.Load(RenderWidth,RenderHeight);
    }

    private int? _renderWidth;
    public int RenderWidth {
        get => _renderWidth ?? Raylib.GetRenderWidth();
        set {
            if (value < 1) _renderWidth = null;
            else _renderWidth = value;
        }
    }

    private int? _renderHeight;
    public int RenderHeight {
        get => _renderHeight ?? Raylib.GetRenderHeight();
        set {
            if (value < 1) _renderHeight = null;
            else _renderHeight = value;
        }
    }

    protected void OnWindowResize() {
        RecreateRenderTexture();
    }

    public abstract Vector2 WorldToScreen(Vector3 position);
    public abstract Vector3 ScreenToWorld(Vector2 position);

    public override void Dispose() {
        base.Dispose();
        IsMain = false;
    }
}