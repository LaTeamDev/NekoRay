using System.Numerics;
using AngleSharp;
using AngleSharp.Css;
using AngleSharp.Css.Dom;
using AngleSharp.Io;
using Yoga;
using ZeroElectric.Vinculum;
using Font = NekoRay.Font;

namespace WebRenderingTest;

public static class Extensions {
    public static void DrawBlock(this Node node, float offsetX = 0, float offsetY = 0) {
        var context = node.Context as WebRenderContext;
        var style = context.Style;

        var colorProp = style.GetProperty("background-color");
        if (colorProp.Value != "") {
            var colorRaw = colorProp.RawValue.AsRgba();
            var color = colorRaw.ToColor();
        
            Raylib.DrawRectangleRec(new Rectangle(node.Left+offsetX, node.Top+offsetY, node.Width, node.Height), color);
        }

        var borderTop = node.GetBorder(Edge.Top);
        var borderBottom = node.GetBorder(Edge.Bottom);
        var borderRight = node.GetBorder(Edge.Right);
        var borderLeft = node.GetBorder(Edge.Left);

        var borderColorRaw = style.GetProperty("border-color").RawValue.AsRgba();
        var borderColor = Raylib.GetColor((uint) borderColorRaw);
        
        //Top
        Raylib.DrawRectangleRec(new Rectangle(node.Left+offsetX, node.Top+offsetY, node.Width, borderTop), borderColor);
        //Left
        Raylib.DrawRectangleRec(new Rectangle(node.Left+offsetX, node.Top+offsetY, borderLeft, node.Height), borderColor);
        //Right
        Raylib.DrawRectangleRec(new Rectangle(node.Left+node.Width-borderRight+offsetX, node.Top+offsetY, borderRight, node.Height), borderColor);
        //Bottom
        Raylib.DrawRectangleRec(new Rectangle(node.Left+offsetX, node.Top+node.Height-borderBottom+offsetY, node.Width, borderBottom), borderColor);
        foreach (var child in node.Children) {
            child.Draw(node.Left+offsetX, node.Top+offsetY);
        }
    }

    public static void DrawText(this Node node, float offsetX = 0, float offsetY = 0) {
        var context = node.Context as WebRenderContext;
        var style = context.Style;
        var textWidget = context.Widget as TextWidget;
        
        var colorRaw = style.GetProperty("color").RawValue.AsRgba();
        var color = colorRaw.ToColor();
        
        Raylib.DrawTextEx(Raylib.GetFontDefault(),  textWidget.Text, new Vector2(node.Left+offsetX, node.Top+offsetY),(float) (style.GetProperty("font-size")?.RawValue?.AsPx(new DefaultRenderDevice
        {
            DeviceHeight = Raylib.GetMonitorHeight(0),
            DeviceWidth = Raylib.GetMonitorWidth(0),
            ViewPortHeight = Raylib.GetRenderHeight(),
            ViewPortWidth = Raylib.GetRenderWidth(),
        }, RenderMode.Undefined) ?? 16f),
            (float) (style.GetProperty("letter-spacing")?.RawValue?.AsPx(new DefaultRenderDevice
            {
                DeviceHeight = Raylib.GetMonitorHeight(0),
                DeviceWidth = Raylib.GetMonitorWidth(0),
                ViewPortHeight = Raylib.GetRenderHeight(),
                ViewPortWidth = Raylib.GetRenderWidth(),
            }, RenderMode.Undefined) ?? 0f), color );
    }

    public static void Draw(this Node node, float offsetX = 0, float offsetY = 0) {
        var context = node.Context as WebRenderContext;
        
        if (context.WidgetType == WidgetType.Default)
            node.DrawBlock(offsetX, offsetY);
        else if (context.WidgetType == WidgetType.Text) {
            node.DrawText(offsetX, offsetY);
        }
    }

    public static Color ToColor(this Int32 hexValue) {
        Color color;

        color.r = (byte) ((hexValue >> 8) & 0xFF);
        color.g = (byte) ((hexValue >> 16) & 0xFF);
        color.b = (byte) ((hexValue >> 24) & 0xFF);
        color.a = (byte) (hexValue & 0xFF);

        return color;
    }

    public static IConfiguration WithFilesystemRequester(this IConfiguration configuration) =>
        configuration.With(new FilesystemRequester());
}