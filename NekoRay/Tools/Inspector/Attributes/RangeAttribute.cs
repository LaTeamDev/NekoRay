namespace NekoRay.Tools;

public class RangeAttribute : Attribute {
    public float Min = 0f;
    public float Max;
    public RangeAttribute(float limit) {
        Max = limit;
    }

    public RangeAttribute(float min, float max) {
        Min = min;
        Max = max;
    }
}