namespace NekoRay.Easings;

public class EaseInCubic : IEasing {
    public float Eval(float x) => x*x*x;
}
public class EaseOutCubic : IEasing {
    public float Eval(float x) => 1 - MathF.Pow(1 - x, 3);
}

public class EaseInOutCubic : IEasing {
    public float Eval(float x) => x < 0.5 ? 4 * x * x * x : 1 - MathF.Pow(-2 * x + 2, 3) / 2;
}