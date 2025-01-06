namespace NekoRay.Easings;

public class EaseInQuart : IEasing {
    public float Eval(float x) => x * x * x * x;
}
public class EaseOutQuart : IEasing {
    public float Eval(float x) =>  1 - MathF.Pow(1 - x, 4);
}

public class EaseInOutQuart : IEasing {
    public float Eval(float x) => x < 0.5 ? 8 * x * x * x * x : 1 - MathF.Pow(-2 * x + 2, 4) / 2;
}