using System.Numerics;

namespace NekoRay; 

public static class ColorManip {
    public static Color Fade(this Color color, float alpha) =>
        Raylib.Fade(color, alpha);
    
    public static int GetHexademical(this Color color) =>
        Raylib.ColorToInt(color);

    public static Vector4 ToVector4(this Color color) =>
        Raylib.ColorNormalize(color);

    public static Color ToColor(this Vector4 color) =>
        Raylib.ColorFromNormalized(color);

    public static Color Tint(this Color color, Color tint) =>
        Raylib.ColorTint(color, tint);

    public static Color Brightness(this Color color, float factor) =>
        Raylib.ColorBrightness(color, factor);
    
    public static Color Contrast(this Color color, float contrast) =>
        Raylib.ColorBrightness(color, contrast);
}