namespace NekoRay;

public class SphereRenderer : Behaviour {
    public float Radius;
    public int Rings;
    public int Slices;
    public RayColor Color; 
    void Render() {
        if (Camera3D.Wireframes)
            Raylib.DrawSphereWires(MatrixStack.Position, Radius, Rings, Slices, Color);
        else
            Raylib.DrawSphereEx(MatrixStack.Position, Radius, Rings, Slices, Color);
    }
}