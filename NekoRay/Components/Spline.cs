using System.Numerics;
using NekoLib.Core;
using ZeroElectric.Vinculum;

namespace NekoRay;

public enum SplineType {
    Linear,
    Basis,
    CatmullRom,
    BezierQuadratic,
    BezierCubic,
}

public class Spline : Behaviour {
    public float Thickness = 1f;
    public IEnumerable<Vector2> Points;
    public SplineType Type;
    public Color Color;

    void Render() {
        switch (Type) {
            case SplineType.Linear:
                Raylib.DrawSplineLinear(Points.ToArray(), Thickness, Color);
                break;
            case SplineType.Basis:
                Raylib.DrawSplineBasis(Points.ToArray(), Thickness, Color);
                break;
            case SplineType.CatmullRom:
                Raylib.DrawSplineCatmullRom(Points.ToArray(), Thickness, Color);
                break;
            case SplineType.BezierQuadratic:
                throw new NotImplementedException();
                //Raylib.DrawSplineBezierQuadratic(Points.ToArray(), Thickness, Color);
                break;
            case SplineType.BezierCubic:
                Raylib.DrawSplineBezierCubic(Points.ToArray(), Thickness, Color);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}