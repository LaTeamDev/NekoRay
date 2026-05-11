namespace NekoRay;

public class GridRenderer : Behaviour {
    public int Slices = 8;
    public float Spacing = 32f;
    void Render() {
        Raylib.DrawGrid(Slices, Spacing);
    }
}