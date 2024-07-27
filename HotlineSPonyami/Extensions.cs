namespace HotlineSPonyami; 

public static class Extensions {
    public static void RemoveByValue<T, T2>(this Dictionary<T, T2> dictionary, T2 value) where T : notnull {
        foreach (var kv in dictionary) {
            if (kv.Value == null || !kv.Value.Equals(value)) continue;
            dictionary.Remove(kv.Key);
        }
    }

    public static Animation ToAnimation(this AnimationFrame[] animationFrames) {
        return new Animation(animationFrames);
    }
}