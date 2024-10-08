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
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static unsafe void RaylibCallback(int msgTypeRaw, sbyte* textPtr, sbyte* args) {
        var text = Marshal.PtrToStringUTF8((IntPtr)textPtr);
        var format = Formatting.ConvertPrintfToCSharp(text);
        var msgType = (TraceLogLevel) msgTypeRaw;
        switch (msgType) {
            case TraceLogLevel.LOG_INFO:
                Log.Information(format.String, format.GetObjectsFromPtr((IntPtr)args));
                break;
            case TraceLogLevel.LOG_TRACE:
                Log.Verbose(format.String, format.GetObjectsFromPtr((IntPtr)args));
                break;
            case TraceLogLevel.LOG_DEBUG:
                Log.Debug(format.String, format.GetObjectsFromPtr((IntPtr)args));
                break;
            case TraceLogLevel.LOG_WARNING:
                Log.Warning(format.String, format.GetObjectsFromPtr((IntPtr)args));
                break;
            case TraceLogLevel.LOG_ERROR:
                Log.Warning(format.String, format.GetObjectsFromPtr((IntPtr)args));
                break;
            case TraceLogLevel.LOG_FATAL:
                Log.Fatal(format.String, format.GetObjectsFromPtr((IntPtr)args));
                break;
        }
    }

    public override void Load(string[] args) {
        
        base.Load(args);

        World.LengthUnitsPerMeter = 64f;
        Physics.DefaultGravity = Vector2.Zero;
        if (!DevMode)
        {
            SceneManager.LoadScene(new TiledScene());
        }
    }
    public override void Draw() {
        base.Draw();
    }

    public override void Update() {
        base.Update();
    }
}
