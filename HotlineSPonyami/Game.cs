using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Box2D;
using HotlineSPonyami.Tools;
using HotlineSPonyami.Interop;
using NekoLib.Scenes;
using NekoRay;
using NekoRay.Physics2D;
using ZeroElectric.Vinculum;
using Console = NekoRay.Tools.Console;

namespace HotlineSPonyami;

public class Game : GameBase {


    public override void Load(string[] args) {
        
        base.Load(args);

        World.LengthUnitsPerMeter = 64f;
        Physics.DefaultGravity = Vector2.Zero;
        if (!DevMode)
        {
            //SceneManager.LoadScene(new TiledScene());
        }
    }
    public override void Draw() {
        base.Draw();
    }

    public override void Update() {
        base.Update();
    }
}
