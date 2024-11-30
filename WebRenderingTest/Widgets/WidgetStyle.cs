using AngleSharp.Css.Dom;
using ZeroElectric.Vinculum;
using Texture = NekoRay.Texture;

namespace WebRenderingTest.Widgets;

public class WidgetStyle {
    public float Top;
    public float Left;

    public float Width;
    public float Height;

    public Color BackgroundColor;
    public BackgroundAttachment BackgroundAttachment;
    public Texture? BackgroundTexture;
    public BackgroundRepeat BackgroundRepeat;

    public Color Color;
}