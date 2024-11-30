using System.Numerics;
using AngleSharp.Css;
using AngleSharp.Css.Dom;
using Yoga;
using ZeroElectric.Vinculum;

namespace WebRenderingTest.Widgets;

public class TextWidget(ICssStyleDeclaration style, Node node, string text) : Widget(style, node) {
    public string Text = text;

    public override void Draw(float x = 0f, float y = 0f) {
        var style = Style;
        var node = Node;
        
        var colorRaw = style.GetProperty("color").RawValue.AsRgba();
        var color = colorRaw.ToColor();
        
        Raylib.DrawTextEx(
            WebRenderingScene.Font._font, 
            Text, 
            new Vector2(node.Left+x, node.Top+y),
            (float) (style.GetProperty("font-size")?.RawValue?.AsPx(WebRenderingScene._renderDevice, RenderMode.Undefined) ?? 16f),
            (float) (style.GetProperty("letter-spacing")?.RawValue?.AsPx(WebRenderingScene._renderDevice, RenderMode.Undefined) ?? 0f), color);
        
    }
}