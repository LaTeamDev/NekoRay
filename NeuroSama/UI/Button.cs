using System.Drawing;
using System.Numerics;
using NekoLib.Core;
using NekoRay;
using NekoRay.Tools;
using Serilog;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace NeuroSama.UI;

public class Button : Behaviour {
    public SizeF Size = new(64f, 64f);
    public Vector2 Origin = new(0.5f, 0.5f);

    private bool _firstFrameHover = true;
    private bool _firstFrameClick = true;
    private RectangleF Rect => new((PointF) (Transform.Position.ToVector2() - Size.ToVector2() * Origin), Size);

    void Update() {
        var mouseDown = Input.IsDown("attack");
        if (!mouseDown && !_firstFrameClick) {
            GameObject.SendMessage("OnButtonReleased", this);
            _firstFrameClick = true;
        }
        if (!NekoMath.CheckPointInRect(Rect,Camera2D.Main.ScreenToWorld(Input.MousePosition).ToVector2())) {
            if (!_firstFrameHover) {
                GameObject.SendMessage("OnButtonMouseLeave", this);
            }
            _firstFrameHover = true;
            return;
        }
        if (_firstFrameHover) {
            GameObject.SendMessage("OnButtonMouseEnter", this);
            _firstFrameHover = false;
        }
        GameObject.SendMessage("OnButtonMouseHover", this);
        if (mouseDown) {
            if (_firstFrameClick) {
                GameObject.SendMessage("OnButtonPressed", this);
                _firstFrameClick = false;
            }
            GameObject.SendMessage("OnButtonDown", this);
        }

        if (!mouseDown && !_firstFrameClick) {
            GameObject.SendMessage("OnButtonClick", this);
        }
    }

    void Render() {
        if (!DrawAabb) return;
        Raylib.DrawRectangleLinesEx(Rect.ToRaylib(), 1f, Raylib.GOLD);
    }

    [ConVariable("r_button_aabb")] [ConTags("cheat")]
    public static bool DrawAabb { get; set; }
    

    void OnButtonMouseEnter(Button sender) {
        Raylib.SetMouseCursor(MouseCursor.MOUSE_CURSOR_POINTING_HAND);
    }
    
    void OnButtonMouseLeave(Button sender) {
        Raylib.SetMouseCursor(MouseCursor.MOUSE_CURSOR_DEFAULT);
    }
}