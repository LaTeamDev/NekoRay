using System.Numerics;

namespace NekoRay;

public struct BezierCurve1D(float p1, float p2, float p3, float p4) {
    public float P0 = p1;
    public float P1 = p2;
    public float P2 = p3;
    public float P3 = p4;

    public float Lerp(float from, float to, float t) {
        var a = float.Lerp(P0, P1, t);
        var b = float.Lerp(P1, P2, t);
        var c = float.Lerp(P2, P3, t);
        var d = float.Lerp(a, b, t);
        var e = float.Lerp(b, c, t);
        return float.Lerp(b, c, t);
    }
}