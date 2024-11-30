using AngleSharp.Css.Dom;
using Yoga;
using ZeroElectric.Vinculum;

namespace WebRenderingTest.Widgets;

public class Widget {
    public ICssStyleDeclaration Style { get; }
    public Node Node { get; }
    public WidgetList Children { get; }
    public Widget? Parent => Node.Parent?.Context as Widget;
    public Widget(ICssStyleDeclaration style, Node node) {
        Style = style;
        Node = node;
        Children = new WidgetList(this);
    }

    public virtual void Draw(float x = 0f, float y = 0f) {
        var style = Style;
        var node = Node;
        
        var colorProp = style.GetProperty("background-color");
        if (colorProp.Value != "") {
            var colorRaw = colorProp.RawValue.AsRgba();
            var color = colorRaw.ToColor();
        
            Raylib.DrawRectangleRec(new Rectangle(node.Left+x, node.Top+y, node.Width, node.Height), color);
        }

        /*
        var borderTop = node.GetBorder(Edge.Top);
        var borderBottom = node.GetBorder(Edge.Bottom);
        var borderRight = node.GetBorder(Edge.Right);
        var borderLeft = node.GetBorder(Edge.Left);

        var borderColorRaw = style.GetProperty("border-color").RawValue.AsRgba();
        var borderColor = Raylib.GetColor((uint) borderColorRaw);
        
        //Top
        Raylib.DrawRectangleRec(new Rectangle(node.Left+x, node.Top+y, node.Width, borderTop), borderColor);
        //Left
        Raylib.DrawRectangleRec(new Rectangle(node.Left+x, node.Top+y, borderLeft, node.Height), borderColor);
        //Right
        Raylib.DrawRectangleRec(new Rectangle(node.Left+node.Width-borderRight+x, node.Top+y, borderRight, node.Height), borderColor);
        //Bottom
        Raylib.DrawRectangleRec(new Rectangle(node.Left+x, node.Top+node.Height-borderBottom+y, node.Width, borderBottom), borderColor);
        */
    }

    public virtual int BuildHashCode() {
        return HashCode.Combine(Style);
    }
}