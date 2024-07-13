using NekoLib.Scenes;
using ZeroElectric.Vinculum;

namespace NekoRay; 

public abstract class GameBase {
    public virtual void Load(string[] args) {
        
    }

    public event Action? WindowResize;
    public event Action<KeyboardKey, bool>? KeyPressed;

    public virtual void UpdateEvents() {
        if (Raylib.IsWindowResized())
            WindowResize?.Invoke();
        foreach (var key in Enum.GetValues<KeyboardKey>()) {
            var keyPressed = Raylib.IsKeyPressed(key);
            var keyPressedRepeat = Raylib.IsKeyPressedRepeat(key);
            if (keyPressed || keyPressedRepeat) {
                KeyPressed?.Invoke(key, keyPressedRepeat);
            }
        }
    }
    public virtual void Update() {
        SceneManager.Update();
    }

    public virtual void Draw() {
        SceneManager.Draw();
    }

    public delegate void LoopFunction();

    public virtual LoopFunction Run(string[] args) {
        Raylib.InitAudioDevice();
        Load(args);
        return () => {
            Timer.Step();
            UpdateEvents();
            Update();
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            Draw();
            Raylib.EndDrawing();
        };
    }

    public virtual LoopFunction ErrorHandler(Exception msg) {
        var error = msg.ToString();
        Console.WriteLine(error);

        void Draw() {
            Raylib.ClearBackground(Raylib.RAYWHITE);
            Raylib.DrawText(error, 70, 70, 10, Raylib.GRAY);
        }

        return () => {
            Raylib.BeginDrawing();
            Draw();
            Raylib.EndDrawing();
            Thread.Sleep(100);
        };
    }
}