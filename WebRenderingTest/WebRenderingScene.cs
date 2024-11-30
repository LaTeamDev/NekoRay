using System.Drawing;
using System.Numerics;
using System.Text;
using AngleSharp;
using AngleSharp.Css;
using AngleSharp.Css.Dom;
using AngleSharp.Css.RenderTree;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using NekoLib.Core;
using NekoRay;
using Serilog;
using WebRenderingTest.Widgets;
using Yoga;
using ZeroElectric.Vinculum;
using Color = System.Drawing.Color;
using FlexDirection = Yoga.FlexDirection;
using Font = NekoRay.Font;
using Node = Yoga.Node;
using NodeType = Yoga.NodeType;

namespace WebRenderingTest;

public class WebRenderingScene : BaseScene {
    public override void Initialize() {
        new GameObject("Html").AddComponent<HtmlRenderer>().OpenPage("file:///html/video.html");
        base.Initialize();
    }

    public override void Draw() {
      base.Draw();
      
    }
}