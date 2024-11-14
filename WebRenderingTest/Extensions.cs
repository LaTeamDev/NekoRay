using System.Numerics;
using AngleSharp.Css.Dom;
using Yoga;
using ZeroElectric.Vinculum;

namespace WebRenderingTest;

public static class Extensions {
    public static void Draw(this YogaNode node, float offsetX = 0, float offsetY = 0) {
        var style = node.UserData as ICssStyleDeclaration;
        
        var colorRaw = style.GetProperty("background-color").RawValue.AsRgba();
        var color = Raylib.GetColor((uint) colorRaw);
        
        Raylib.DrawRectangleRec(new Rectangle(node.Left+offsetX, node.Top+offsetY, node.Width, node.Height), color);
        
        var borderTop = node.GetBorder(YogaEdge.Top);
        var borderBottom = node.GetBorder(YogaEdge.Bottom);
        var borderRight = node.GetBorder(YogaEdge.Right);
        var borderLeft = node.GetBorder(YogaEdge.Left);

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
            if (child.Type == YogaNodeType.Default)
                child.Draw(offsetX+node.Left, offsetY+node.Top);
            //else
              //  ((TextNode)child).Draw(new Vector2(offsetX+node.Left, offsetY+node.Top));
        }
    }
}