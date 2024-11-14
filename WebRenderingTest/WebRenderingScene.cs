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

namespace WebRenderingTest;

public class WebRenderingScene : BaseScene {
    public override void Initialize() {
        var source = @"<html>
  <head>
    <style>
    .flex {
  display:flex;
  align-items: stretch;
  align-content: stretch;
  gap: 4px;
  flex-direction: row;
}

.flex-col {
  flex-direction: column;
}

#main {
  width: 128px;
  background-color: rgba (128,128,128,1);
  text-align: center;
  padding: 8px;
  border-radius: 2px;
}

div {
  background: rgba(255,255,255,0.25);
  border: 1px solid purple;
  min-height: 16px;
}
    </style>
  </head>
  <body>
    <div id=""main"" class=""flex flex-col"">
      <div>a</div>
      <div>b</div>
      <div class=""flex"">
        <div style=""width:16px"">1</div>
        <div style=""width:36px"">2</div>
      </div>
      <div>d</div>
    </div>
  </body>
</html>";
        IConfiguration config = Configuration.Default.WithCss().WithRenderDevice(new DefaultRenderDevice
        {
          DeviceHeight = Raylib.GetMonitorHeight(0),
          DeviceWidth = Raylib.GetMonitorWidth(0),
          ViewPortHeight = Raylib.GetRenderHeight(),
          ViewPortWidth = Raylib.GetRenderWidth(),
        });

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
        _body.Children[0].Children[0].Print(YogaPrintOptions.Style);
        base.Initialize();
    }

    public override void Draw() {
      base.Draw();
      _body.Draw();
    }

    private YogaConfig yogaConfig = YogaConfig.Default;
    private YogaNode _body;

    private YogaNode CreateLayoutNode(ICssStyleDeclaration style) {
      var nodeLayout = new YogaNode(yogaConfig);
      SetWidth(nodeLayout, style.GetWidth());
      SetHeight(nodeLayout, style.GetHeight());
      SetMinHeight(nodeLayout, style.GetMinHeight());
      SetFlexDirection(nodeLayout, style.GetFlexDirection());
      nodeLayout.AlignContent = GetAlign(style.GetAlignContent())??nodeLayout.AlignContent;
      nodeLayout.AlignItems = GetAlign(style.GetAlignItems())??nodeLayout.AlignItems;
      nodeLayout.AlignSelf = GetAlign(style.GetAlignSelf())??nodeLayout.AlignSelf;
      nodeLayout.JustifyContent = GetJustify(style.GetJustifyContent())??nodeLayout.JustifyContent;
      nodeLayout.FlexWrap = GetWrap(style.GetFlexWrap())??nodeLayout.FlexWrap;
      SetMargin(nodeLayout, style.GetMarginBottom(), YogaEdge.Bottom);
      SetMargin(nodeLayout, style.GetMarginTop(), YogaEdge.Top);
      SetMargin(nodeLayout, style.GetMarginLeft(), YogaEdge.Left);
      SetMargin(nodeLayout, style.GetMarginRight(), YogaEdge.Right);
      
      SetPadding(nodeLayout, style.GetPaddingBottom(), YogaEdge.Bottom);
      SetPadding(nodeLayout, style.GetPaddingTop(), YogaEdge.Top);
      SetPadding(nodeLayout, style.GetPaddingLeft(), YogaEdge.Left);
      SetPadding(nodeLayout, style.GetPaddingRight(), YogaEdge.Right);
      
      SetBorder(nodeLayout, style.GetBorderBottom(), YogaEdge.Bottom);
      SetBorder(nodeLayout, style.GetBorderTop(), YogaEdge.Top);
      SetBorder(nodeLayout, style.GetBorderLeft(), YogaEdge.Left);
      SetBorder(nodeLayout, style.GetBorderRight(), YogaEdge.Right);
      SetGap(nodeLayout, style.GetColumnGap(), YogaGutter.All); //FIXME: no row gap??
      nodeLayout.UserData = style;
      return nodeLayout;
    }

    private YogaNode CreateLayoutTextNode() {
      return new YogaNode(yogaConfig) {Type = YogaNodeType.Text};
    }
    private YogaNode CreateLayoutNodeTree(IWindow window, IRenderNode node) {
      var htmlElement = node.Ref as IHtmlElement;
      if (htmlElement is null) {
        //return CreateLayoutTextNode();
      }
      var element = CreateLayoutNode(window.GetComputedStyle(node.Ref as IHtmlElement));
      if (node.Children == null) return element;
      foreach (var child in node.Children) {
        if (child.Ref is IText) continue;
        var childLayout = CreateLayoutNodeTree(window, child);
        childLayout.Parent = element;
      }
      return element;
    }

    private void SetWidth(YogaNode node, string? width) {
      if (width is null) return;
      if (width.EndsWith("%")) node.StyleSetWidthPercent(Convert.ToSingle(width.Substring(0, width.Length-1)));
      if (width.EndsWith("px")) node.Width = (Convert.ToSingle(width.Substring(0, width.Length-2)));
      if (width.EndsWith("auto")) node.StyleSetWidthAuto();
    }
    
    private void SetHeight(YogaNode node, string? height) {
      if (height is null) return;
      if (height.EndsWith("%")) node.StyleSetHeightPercent(Convert.ToSingle(height.Substring(0, height.Length-1)));
      if (height.EndsWith("px")) node.Height = (Convert.ToSingle(height.Substring(0, height.Length-2)));
      if (height.EndsWith("auto")) node.StyleSetHeightAuto();
    }
    
    private void SetMinHeight(YogaNode node, string? minHeight) {
      if (minHeight is null) return;
      if (minHeight.EndsWith("%")) node.StyleSetMinHeightPercent(Convert.ToSingle(minHeight.Substring(0, minHeight.Length-1)));
      if (minHeight.EndsWith("px")) node.StyleSetMinHeight(Convert.ToSingle(minHeight.Substring(0, minHeight.Length-2)));
    }
    
    private void SetMargin(YogaNode node, string? margin, YogaEdge edge) {
      if (margin is null) return;
      if (margin.EndsWith("%")) node.StyleSetMarginPercent(edge, Convert.ToSingle(margin.Substring(0, margin.Length-1)));
      if (margin.EndsWith("px")) node.StyleSetMargin(edge, Convert.ToSingle(margin.Substring(0, margin.Length-2)));
      if (margin.EndsWith("auto")) node.StyleSetMarginAuto(edge);
    }
    
    private void SetPadding(YogaNode node, string? padding, YogaEdge edge) {
      if (padding is null) return;
      if (padding.EndsWith("%")) node.StyleSetPaddingPercent(edge, Convert.ToSingle(padding.Substring(0, padding.Length-1)));
      if (padding.EndsWith("px")) node.StyleSetPadding(edge, Convert.ToSingle(padding.Substring(0, padding.Length-2)));
    }
    
    private void SetBorder(YogaNode node, string? height, YogaEdge edge) {
      if (height is null) return;
      if (height.EndsWith("px")) node.StyleSetBorder(edge, Convert.ToSingle(height.Substring(0, height.Length-2)));
    }
    
    private void SetGap(YogaNode node, string? width, YogaGutter gutter) {
      if (width is null) return;
      if (width.EndsWith("px")) node.StyleSetGap(Convert.ToSingle(width.Substring(0, width.Length-2)), gutter);
    }

    private void SetDisplay(YogaNode node, string? display) {
      if (display is null) return;
      if (display == "none") node.Display = YogaDisplay.None;
    }
    
    private void SetFlexDirection(YogaNode node, string? flexdirection) {
      if (flexdirection is null) return;
      if (Enum.TryParse(typeof(YogaFlexDirection), flexdirection.Replace("-",""),true, out var yogaDirection))
        node.FlexDirection = (YogaFlexDirection)yogaDirection;
    }

    private YogaAlign? GetAlign(string? align) {
      if (align is null) return null;
      if (Enum.TryParse(typeof(YogaAlign), align.Replace("-",""),true, out var yogaAlign))
        return (YogaAlign)yogaAlign;
      return null;
    }
    
    private YogaJustify? GetJustify(string? justify) {
      if (justify is null) return null;
      if (Enum.TryParse(typeof(YogaJustify), justify.Replace("-",""),true, out var yogaJustify))
        return (YogaJustify)yogaJustify;
      return null;
    }
    
    private YogaWrap? GetWrap(string? justify) {
      if (justify is null) return null;
      if (Enum.TryParse(typeof(YogaWrap), justify.Replace("-",""),true, out var yogaJustify))
        return (YogaWrap)yogaJustify;
      return null;
    }
}