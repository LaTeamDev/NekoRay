using System.Drawing;
using System.Text;
using AngleSharp;
using AngleSharp.Css;
using AngleSharp.Css.Dom;
using AngleSharp.Css.RenderTree;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using NekoRay;
using Serilog;
using Yoga;
using ZeroElectric.Vinculum;
using FlexDirection = Yoga.FlexDirection;
using Font = NekoRay.Font;
using Node = Yoga.Node;
using NodeType = Yoga.NodeType;

namespace WebRenderingTest;

public class WebRenderingScene : BaseScene {
    public override void Initialize() {
        var source = @"<html>
  <head>
<style>
div {
  display: flex;
  flex-direction: column;
  box-sizing: border-box;
}

body {
  height: 100vh;
  overflow: hidden;
  background-color: #121212;
  color: white;
  margin: 0;
}
#app {
  width: 100vw;
  overflow: auto;
  height: 100%;
}

#navbar {
  height: 3rem;
  background-color: rgba(255, 255, 255, 0.2);
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  padding-left: 1rem;
  padding-right: 1rem;
}

#main {
  width: 48rem;
  height: 100%;
}

#sidebar {
  width: 16rem;
  gap: 1rem;
}

#contents {
  justify-content: space-between;
  flex-direction: row;
  padding-left: 4rem;
  padding-right: 4rem;
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
  height: 100%;
  
}

.card {
  background-color: rgba(255, 255, 255, 0.1);
    padding: 1rem;
}

#video {
  width: 100%;
  aspect-ratio: 1.77;
  background-color: black;
}

.video-title {
  font-size: 24px;
}

.suggestion {
  flex-direction: row;
  gap: 0.5rem;
}

.preview {
  width: 7rem;
  height: 4rem;
  /*background-color: rgba(0,0,0,0.25);*/
}

.preview-title {
  margin: 0;
  font-size: 1rem;
}
</style>
</head>
  <body>
    <div id=""app"">
      <div id=""navbar"">
        <div>cool design 228</div>
        <div>Login</div>
      </div>
        <div id=""contents"">
          <div id=""main"" class=""card"">
            <div id=""video""></div>
            <h1 class=""video-title"">Wow what a cool video</div>
          </d>
          <div id=""sidebar"" class=""card"">
            <div class=""suggestion"">
              <div class=""preview""></div>
              <h1 class=""preview-title"">what a lame video</h1>
            </div>
            <div class=""suggestion"">
              <div class=""preview""></div>
              <h1 class=""preview-title"">what a lame video</h1>
            </div>
            <div class=""suggestion"">
              <div class=""preview""></div>
              <h1 class=""preview-title"">what a lame video</h1>
            </div>
            <div class=""suggestion"">
              <div class=""preview""></div>
              <h1 class=""preview-title"">what a lame video</h1>
            </div>
            <div class=""suggestion"">
              <div class=""preview""></div>
              <h1 class=""preview-title"">what a lame video</h1>
            </div>
          </div>
        </div>
    </div>
  </body>
</html>";
        _renderDevice = new DefaultRenderDevice
        {
          DeviceHeight = Raylib.GetMonitorHeight(0),
          DeviceWidth = Raylib.GetMonitorWidth(0),
          ViewPortHeight = Raylib.GetRenderHeight(),
          ViewPortWidth = Raylib.GetRenderWidth(),
        };
        IConfiguration config = Configuration.Default.WithCss().WithRenderDevice(_renderDevice);

        //Create a new context for evaluating webpages with the given config
        IBrowsingContext context = BrowsingContext.New(config);

        //Just get the DOM representation
        IDocument document = context.OpenAsync(req => req.Content(source)).Result;
        var window = document.DefaultView;
        var render = window.Render();
        //var style = context.GetCssStyling().ParseStylesheetAsync(new DefaultResponse{Content = new MemoryStream( Encoding.UTF8.GetBytes( style ) )}, new StyleOptions(context.Active), CancellationToken.None).Result;
        //document.style;
        
        var body_render = render.Find(document.QuerySelector("body"));
        _body = CreateLayoutNodeTree(window, body_render);
        _body.CalculateLayout(Raylib.GetRenderWidth(), Raylib.GetRenderHeight());
        //_body.Children[1].Children[0].Print(PrintOptions.Layout);
        base.Initialize();
    }

    public override void Draw() {
      base.Draw();
      _body.Draw();
    }

    private YogaConfig yogaConfig = YogaConfig.Default;
    private Node _body;
    private DefaultRenderDevice _renderDevice;

    private Node CreateLayoutNode(ICssStyleDeclaration style) {
      var nodeLayout = new Node(yogaConfig);
      SetWidth(nodeLayout, style.GetWidth());
      SetHeight(nodeLayout, style.GetHeight());
      SetMinHeight(nodeLayout, style.GetMinHeight());
      SetFlexDirection(nodeLayout, style.GetFlexDirection());
      nodeLayout.AlignContent = GetAlign(style.GetAlignContent())??nodeLayout.AlignContent;
      nodeLayout.AlignItems = GetAlign(style.GetAlignItems())??nodeLayout.AlignItems;
      nodeLayout.AlignSelf = GetAlign(style.GetAlignSelf())??nodeLayout.AlignSelf;
      nodeLayout.JustifyContent = GetJustify(style.GetJustifyContent())??nodeLayout.JustifyContent;
      nodeLayout.FlexWrap = GetWrap(style.GetFlexWrap())??nodeLayout.FlexWrap;
      SetMargin(nodeLayout, style.GetMarginBottom(), Edge.Bottom);
      SetMargin(nodeLayout, style.GetMarginTop(), Edge.Top);
      SetMargin(nodeLayout, style.GetMarginLeft(), Edge.Left);
      SetMargin(nodeLayout, style.GetMarginRight(), Edge.Right);
      
      SetPadding(nodeLayout, style.GetPaddingBottom(), Edge.Bottom);
      SetPadding(nodeLayout, style.GetPaddingTop(), Edge.Top);
      SetPadding(nodeLayout, style.GetPaddingLeft(), Edge.Left);
      SetPadding(nodeLayout, style.GetPaddingRight(), Edge.Right);
      
      SetBorder(nodeLayout, style.GetBorderBottom(), Edge.Bottom);
      SetBorder(nodeLayout, style.GetBorderTop(), Edge.Top);
      SetBorder(nodeLayout, style.GetBorderLeft(), Edge.Left);
      SetBorder(nodeLayout, style.GetBorderRight(), Edge.Right);
      SetGap(nodeLayout, style.GetColumnGap(), Gutter.All); //FIXME: no row gap??
      nodeLayout.Context = new WebRenderContext {
        Style = style,
        Widget = new Widget(),
        WidgetType = WidgetType.Default
      };
      var aspectRatio = style.GetProperty("aspect-ratio");
      if (aspectRatio?.RawValue != null)
        nodeLayout.AspectRatio = (float) aspectRatio.RawValue.AsDouble();
      return nodeLayout;
    }

    public static Font Font = Font.Default;
    private Node CreateLayoutTextNode(ICssStyleDeclaration style, string text) {
      return new Node(yogaConfig) {
        Type = NodeType.Text,
        Context = new WebRenderContext {
          Style = style,
          Widget = new TextWidget(text),
          WidgetType = WidgetType.Text
        },
        MeasureFunction = (node, width, mode, height, heightMode) => {
          Raylib.TextLength(text);
          var sizeV = Font.Measure(text,
            (float) (style.GetProperty("font-size")?.RawValue?.AsPx(_renderDevice, RenderMode.Undefined) ?? 16f),
            (float) (style.GetProperty("letter-spacing")?.RawValue?.AsPx(_renderDevice, RenderMode.Undefined) ?? 0f));
          return new SizeF(sizeV);
        }
      };
    }
    private Node CreateLayoutNodeTree(IWindow window, IRenderNode node) {
      var htmlElement = node.Ref as IHtmlElement;
      if (htmlElement is null) {
        var el = node.Ref as IText;
        var text = el.Text.Trim('\n').Trim();
        return CreateLayoutTextNode(window.GetComputedStyle(el.Parent as IHtmlElement), el.Text);
      }
      var element = CreateLayoutNode(window.GetComputedStyle(node.Ref as IHtmlElement));
      if (node.Children == null) return element;
      foreach (var child in node.Children) {
        if (child.Ref is IText el) {
          if (el.Text.Trim('\n').Trim() == "") continue;
        }
        var childLayout = CreateLayoutNodeTree(window, child);
        childLayout.Parent = element;
      }
      return element;
    }

    private void SetWidth(Node node, string? width) {
      if (width is null) return;
      if (width.EndsWith("%")) node.StyleSetWidthPercent(Convert.ToSingle(width.Substring(0, width.Length-1)));
      if (width.EndsWith("px")) node.Width = (Convert.ToSingle(width.Substring(0, width.Length-2)));
      if (width.EndsWith("auto")) node.StyleSetWidthAuto();
    }
    
    private void SetHeight(Node node, string? height) {
      if (height is null) return;
      if (height.EndsWith("%")) node.StyleSetHeightPercent(Convert.ToSingle(height.Substring(0, height.Length-1)));
      if (height.EndsWith("px")) node.Height = (Convert.ToSingle(height.Substring(0, height.Length-2)));
      if (height.EndsWith("auto")) node.StyleSetHeightAuto();
    }
    
    private void SetMinHeight(Node node, string? minHeight) {
      if (minHeight is null) return;
      if (minHeight.EndsWith("%")) node.StyleSetMinHeightPercent(Convert.ToSingle(minHeight.Substring(0, minHeight.Length-1)));
      if (minHeight.EndsWith("px")) node.StyleSetMinHeight(Convert.ToSingle(minHeight.Substring(0, minHeight.Length-2)));
    }
    
    private void SetMargin(Node node, string? margin, Edge edge) {
      if (margin is null) return;
      if (margin.EndsWith("%")) node.StyleSetMarginPercent(edge, Convert.ToSingle(margin.Substring(0, margin.Length-1)));
      if (margin.EndsWith("px")) node.StyleSetMargin(edge, Convert.ToSingle(margin.Substring(0, margin.Length-2)));
      if (margin.EndsWith("auto")) node.StyleSetMarginAuto(edge);
    }
    
    private void SetPadding(Node node, string? padding, Edge edge) {
      if (padding is null) return;
      if (padding.EndsWith("%")) node.StyleSetPaddingPercent(edge, Convert.ToSingle(padding.Substring(0, padding.Length-1)));
      if (padding.EndsWith("px")) node.StyleSetPadding(edge, Convert.ToSingle(padding.Substring(0, padding.Length-2)));
    }
    
    private void SetBorder(Node node, string? height, Edge edge) {
      if (height is null) return;
      if (height.EndsWith("px")) node.StyleSetBorder(edge, Convert.ToSingle(height.Substring(0, height.Length-2)));
    }
    
    private void SetGap(Node node, string? width, Gutter gutter) {
      if (width is null) return;
      if (width.EndsWith("px")) node.StyleSetGap(Convert.ToSingle(width.Substring(0, width.Length-2)), gutter);
    }

    private void SetDisplay(Node node, string? display) {
      if (display is null) return;
      if (display == "none") node.Display = Display.None;
    }
    
    private void SetFlexDirection(Node node, string? flexdirection) {
      if (flexdirection is null) return;
      if (Enum.TryParse(typeof(FlexDirection), flexdirection.Replace("-",""),true, out var yogaDirection))
        node.FlexDirection = (FlexDirection)yogaDirection;
    }

    private Align? GetAlign(string? align) {
      if (align is null) return null;
      if (Enum.TryParse(typeof(Align), align.Replace("-",""),true, out var yogaAlign))
        return (Align)yogaAlign;
      return null;
    }
    
    private Justify? GetJustify(string? justify) {
      if (justify is null) return null;
      if (Enum.TryParse(typeof(Justify), justify.Replace("-",""),true, out var yogaJustify))
        return (Justify)yogaJustify;
      return null;
    }
    
    private Wrap? GetWrap(string? justify) {
      if (justify is null) return null;
      if (Enum.TryParse(typeof(Wrap), justify.Replace("-",""),true, out var yogaJustify))
        return (Wrap)yogaJustify;
      return null;
    }
}