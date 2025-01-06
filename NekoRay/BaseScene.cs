using System.Numerics;
using NekoRay.Physics2D;
using Serilog;

namespace NekoRay; 

public abstract class BaseScene : IScene {
    public virtual string Name => GetType().Name;
    public bool DestroyOnLoad { get; protected set; } = true;
    public int Index { get; set; }
    public List<GameObject> GameObjects { get; } = new();
    public virtual void Initialize() {
        var currentGameObjects = new GameObject[GameObjects.Count];
        GameObjects.CopyTo(currentGameObjects);
        foreach (var gameObject in currentGameObjects) {
            gameObject.Initialize();
        }
    }

    public virtual void Update() {
        var currentGameObjects = new GameObject[GameObjects.Count];
        GameObjects.CopyTo(currentGameObjects);
        foreach (var gameObject in currentGameObjects) {
            gameObject.Update();
        }
    }

    public virtual void DrawCameraTexture() {
        if (CliOptions.Instance.DevMode) return;
        if (BaseCamera.Main is null) return;
        if (this != BaseCamera.Main.GameObject.Scene) return;
        
        var texture = BaseCamera.Main.RenderTexture.Texture;
        var rect = new Rectangle(0, 0, texture.Width, -texture.Height);
        var rectDest = new Rectangle(0, 0, texture.Width, texture.Height);
        texture.Draw(rect, rectDest, Vector2.Zero, 0f, Raylib.WHITE);
    }

    public virtual void Draw() {
        var currentGameObjects = new GameObject[GameObjects.Count];
        GameObjects.CopyTo(currentGameObjects);
        foreach (var gameObject in currentGameObjects) {
            gameObject.SendMessage("Draw");
        }
        foreach (var gameObject in currentGameObjects) {
            gameObject.SendMessage("LateDraw");
        }
        DrawCameraTexture();
    }

    public virtual void OnWindowResize() {
        var currentGameObjects = new GameObject[GameObjects.Count];
        GameObjects.CopyTo(currentGameObjects);
        Log.Verbose("Window resized");
        foreach (var gameObject in currentGameObjects) {
            gameObject.SendMessage("OnWindowResize");
        }
    }

    public virtual void FixedUpdate() {
        var currentGameObjects = new GameObject[GameObjects.Count];
        GameObjects.CopyTo(currentGameObjects);
        foreach (var gameObject in currentGameObjects) {
            gameObject.SendMessage("FixedUpdate");
        }
    }

    public virtual void DrawGui() {
        var currentGameObjects = new GameObject[GameObjects.Count];
        GameObjects.CopyTo(currentGameObjects);
        foreach (var gameObject in currentGameObjects) {
            gameObject.SendMessage("DrawGui");
        }
    }

    public virtual void Dispose() {
        var currentGameObjects = new GameObject[GameObjects.Count];
        GameObjects.CopyTo(currentGameObjects);
        foreach (var gameObject in currentGameObjects) {
            gameObject.Dispose();
        }
    }
}