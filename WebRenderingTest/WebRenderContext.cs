using AngleSharp.Css.Dom;

namespace WebRenderingTest;

public class WebRenderContext {
    public ICssStyleDeclaration Style;
    public Widget Widget;
    public WidgetType WidgetType = WidgetType.Default;
}