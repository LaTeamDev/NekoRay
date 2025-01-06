using System.Numerics;

namespace NekoRay;

public struct Range<T> (T min, T max) : IEquatable<Range<T>> {
    public T Min = min;
    public T Max = max;
    

    public override string ToString() => base.ToString()+$"{{{Min},{Max}}}";

    public bool Equals(Range<T> other) => EqualityComparer<T>.Default.Equals(Min, other.Min) && EqualityComparer<T>.Default.Equals(Max, other.Max);

    public override bool Equals(object? obj) => obj is Range<T> other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Min, Max);

    public static bool operator ==(Range<T> left, Range<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Range<T> left, Range<T> right)
    {
        return !(left == right);
    }
}

public static class RangeExtensions {
    public static Vector2 ToVector2(this Range<float> range) => new(range.Min, range.Max);
    public static Range<float> ToRange(this Vector2 range) => new(range.X, range.Y);

    public static T GetRandomValueInBetween<T>(this Random random, Range<T> range) where T : IAdditionOperators<T, T, T>, 
        ISubtractionOperators<T, T, T>, IMultiplyOperators<T, float, T> =>
        range.Min + (range.Max - range.Min) * random.NextSingle();
}