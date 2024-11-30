using System.Numerics;
using System.Runtime.InteropServices;
using AngleSharp;
using AngleSharp.Css;
using AngleSharp.Css.Dom;
using AngleSharp.Io;
using WebRenderingTest.Widgets;
using Yoga;
using ZeroElectric.Vinculum;
using Font = NekoRay.Font;

namespace WebRenderingTest;

public static class Extensions {

    public static List<Node> GetAllNodes(this Node node, List<Node> nodes) {
        nodes.Add(node);
        foreach (var childNode in node.Children) {
            childNode.GetAllNodes(nodes);
        }
        return nodes;
    }
    public static void Draw(this Node node, float x = 0f, float y = 0f) {
        if (node.Context is Widget widget) {
            widget.Draw(x, y);
        }
        foreach (var child in node.Children) {
            child.Draw(node.Left+x, node.Top+y);
        }
    }

    public static float GetAbsoluteTop(this Node node) {
        var value = node.Top;
        foreach (var child in node.Children) {
            value += child.GetAbsoluteTop();
        }
        return value;
    }
    public static float GetAbsoluteLeft(this Node node) {
        var value = node.Left;
        foreach (var child in node.Children) {
            value += child.GetAbsoluteLeft();
        }
        return value;
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
    
    public static GCHandle GcPin<T>(this T obj) => GCHandle.Alloc(obj, GCHandleType.Pinned);
}