using System.Numerics;
using NekoLib.Core;
using NekoRay;
using ZeroElectric.Vinculum;
using Timer = NekoRay.Timer;

namespace HotlineSPonyami.Tools;

public class DragCamera : Behaviour 
{
    private ZeroElectric.Vinculum.Camera2D _camera;
    public ZeroElectric.Vinculum.Camera2D Camera => _camera; // why not just use camera2d component/extend it?

    private Vector2 _mouseStart;

    public float Zoom {
        get => Camera.zoom;
        set => _camera.zoom = value;
    }

    private void Start()
    {
        _camera.target = new Vector2(0, 0);
        _camera.zoom = 1;
        _camera.rotation = 0;
    }
    
    private void Update()
    {
        _camera.offset = new Vector2(Raylib.GetRenderWidth() / 2f, Raylib.GetRenderHeight() / 2f);
        _camera.target = Transform.Position.ToVector2();

        _camera.zoom += Raylib.GetMouseWheelMove() * _camera.zoom / 10f;
        if (Raylib.GetMouseWheelMove() != 0 && Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL))
        {
            _camera.zoom = MathF.Round(_camera.zoom * 5f) / 5f;
        }
        _camera.zoom = Math.Clamp(_camera.zoom, 0.1f, 5f);

        if (Raylib.IsMouseButtonPressed(1))
        {
            _mouseStart = ScreenToWorld(Raylib.GetMousePosition());
        }
        if (Raylib.IsMouseButtonDown(1))
        {
            Vector2 difference = ScreenToWorld(Raylib.GetMousePosition()) - _mouseStart;
            Transform.Position -= new Vector3(difference, 0f);
        }
    }
    
    public Vector2 WorldToScreen(Vector3 position) {
        return Raylib.GetWorldToScreen2D(position.ToVector2(), _camera);
    }
    
    public Vector2 ScreenToWorld(Vector2 position) {
        return Raylib.GetScreenToWorld2D(position, _camera);
    }
}