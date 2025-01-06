namespace NekoRay.Easings;

public class EaseInBack : IEasing {
    public float c1 => 1.70158f;
    public float c3 => c1 + 1;

    public float Eval(float x) => c3 * x * x * x - c1 * x * x;
}
public class EaseOutBack : IEasing {
    public float c1 => 1.70158f;
    public float c2 => c1 * 1.525f;
    public float c3 => c1 + 1;

    public float Eval(float x) => 1 + c3 * MathF.Pow(x - 1, 3) + c1 * MathF.Pow(x - 1, 2);
}

public class EaseInOutBack : IEasing {
    public float c1 => 1.70158f;
    public float c2 => c1 * 1.525f;
    
    public float Eval(float x) => x < 0.5
        ? (MathF.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
        : (MathF.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
}