namespace NekoRay.Easings;

public class EaseInQuad : IEasing {
    public float Eval(float x) => x*x;
}
public class EaseOutQuad : IEasing {
    public float Eval(float x) => 1 - (1 - x) * (1 - x);
}

public class EaseInOutQuad : IEasing {
    public float Eval(float x) => x < 0.5 ? 2 * x * x : 1 - MathF.Pow(-2 * x + 2, 2) / 2;
}