using System.Numerics;
using FlappyPegasus.Gui;
using NekoLib.Core;
using NekoRay;
using ZeroElectric.Vinculum;
using Texture = NekoRay.Texture;

namespace HotlineSPonyami;

public class TiledScene : BaseScene
{
    private Texture _uvTexture;
    private Texture _tileAtlas;
    public TiledScene()
    {
        _tileAtlas = Data.GetTexture("data/textures/texture_atlas.png");
    }
    public override void Initialize() {
        new GameObject("meow").AddComponent<ImguiDemoWindow>();
        base.Initialize();
        
    }

    public override void Draw()
    {
        base.Draw();
    }
}