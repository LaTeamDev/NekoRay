namespace NekoRay; 

public class NoGame : GameBase {
    public override void Draw() {
        Raylib.DrawText("No Game", 0, 0, 11, Raylib.WHITE);
    }
}