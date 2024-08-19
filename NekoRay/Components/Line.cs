using System.Numerics;

namespace NekoRay; 

public class Line : Behaviour {
    public bool UseGl = false;
    public Vector2 StartPosition = Vector2.Zero;
    public Vector2 EndPosition = Vector2.Zero;
    public Color Color = Raylib.WHITE;
    public float Thickness = 1f;
    
    void Render() {
        if (UseGl) Raylib.DrawLineV(StartPosition, EndPosition, Color); 
        else Raylib.DrawLineEx(StartPosition, EndPosition, Thickness, Color);
    }
}