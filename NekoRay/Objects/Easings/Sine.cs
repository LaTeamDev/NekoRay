namespace NekoRay.Easings;

public class EaseInSine : IEasing {
    public float Eval(float x) => 1 - MathF.Cos(x * MathF.PI / 2);
}
public class EaseOutSine : IEasing {
    public float Eval(float x) => MathF.Sin(x * MathF.PI / 2);
}

public class EaseInOutSine : IEasing {
    public float Eval(float x) => -(MathF.Cos(MathF.PI * x) - 1) / 2;
}