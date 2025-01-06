namespace NekoRay.Easings;

public class EaseInQuint : IEasing {
    public float Eval(float x) => x*x*x*x*x;
}
public class EaseOutQuint : IEasing {
    public float Eval(float x) => 1 - MathF.Pow(1 - x, 5);
}

public class EaseInOutQuint : IEasing {
    public float Eval(float x) => x < 0.5 ? 16 * x * x * x * x * x : 1 - MathF.Pow(-2 * x + 2, 5) / 2;
}