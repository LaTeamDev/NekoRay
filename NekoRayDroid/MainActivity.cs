using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Android.Content.PM;
using Android.Content.Res;
using Android.Hardware.Display;
using Android.Util;
using Android.Views;
using Java.Lang;
using NekoRay;
using ZeroElectric.Vinculum;
using ZeroElectric.Vinculum.Extensions;
using Activity = Android.App.Activity;

namespace NekoRayDroid;

[MetaData("android.app.lib_name", Value = "raylib")]
[Activity(Label = "@string/app_name", MainLauncher = true, ScreenOrientation = ScreenOrientation.Landscape)]
public class RaylibActivity : NativeActivity {

    private static RaylibActivity _instance;
    public static RaylibActivity Instance => _instance;

    private static GameBase GetGame(string Identity) {
        return new NoGame();
        GameBase gameBase;
        if (!File.Exists(Identity + ".dll")) return new NoGame();
        var gameDll = Assembly.LoadFrom(Identity+".dll");
        var gameType = gameDll.GetTypes().FirstOrDefault(type => type.IsAssignableTo(typeof(GameBase)));
        if (gameType is null)
            gameBase = new NoGame();
        else
            gameBase = (GameBase)Activator.CreateInstance(gameType);
        return gameBase;
    }
    
    protected override unsafe void OnCreate(Bundle? savedInstanceState) {
        _instance = this;
        JavaSystem.LoadLibrary("raylib");
        RaylibSetAndroidCallback(OnReady);
        Raylib.SetLoadFileDataCallback(&OnFileLoad);
        Raylib.SetLoadFileTextCallback(&OnTextLoad);
        base.OnCreate(savedInstanceState);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    unsafe static byte* OnFileLoad(sbyte* filepathRaw, int* dataSize) {
        string filepath = Marshal.PtrToStringUTF8((IntPtr)filepathRaw);
        if (filepath is null) return null;
        
        var filename = Path.GetFileName(filepath);
        
        using var stream = Instance.Assets.Open(filename);
        if (stream is null) {
            Log.Warn("AssetLoading", $"FILEIO: [{filename}] Failed to open file");
            return null;
        }
        
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        if (*dataSize == 0) *dataSize = (int)ms.Length;
        
        if (ms.Length > 2147483647)
        {
            Log.Warn("AssetLoading", "FILEIO: [%s] File is bigger than 2147483647 bytes, avoid using LoadFileData()", filename);
            return null;
        }
        
        byte* data = null;

        if (*dataSize <= 0) {
            Log.Warn("AssetLoading", $"FILEIO: [{filename}] Failed to read file");
            return data;
        }

        data = (byte*)Marshal.AllocHGlobal(*dataSize).ToPointer();

        if (data is null) {
            Log.Warn("AssetLoading", $"FILEIO: [{filename}] Failed to allocated memory for file reading");
            return data;
        }
        Marshal.Copy(ms.ToArray(), 0, (IntPtr)data, *dataSize);
        
        //*dataSize = (int)count;

        //if ((*dataSize) != size) Log.Warn("AssetLoading", $"FILEIO: [{filename}] File partially loaded ({dataSize} bytes out of {count})");
        //else
        Log.Info("AssetLoading", $"FILEIO: [{filename}] File loaded successfully");

        return data;
    }
    
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    unsafe static sbyte* OnTextLoad(sbyte* filepathRaw) {
        sbyte* text = null;
        var filepath = Marshal.PtrToStringUTF8((IntPtr)filepathRaw);

        if (filepath is null)
        {
            Log.Warn("AssetLoading", "FILEIO: File name provided is not valid");
            return text;
        }
        
        var filename = Path.GetFileName(filepath);
        
        using var stream = Instance.Assets.Open(filename);

        if (stream is null)
        {
            Log.Warn("AssetLoading", $"FILEIO: [{filename}] Failed to open text file");
            return text;
        }

        using var streamReader = new StreamReader(stream);
        var str = streamReader.ReadToEnd();

        if (str.Length <= 0)
        {
            Log.Info("AssetLoading", $"FILEIO: [{filename}] Failed to read text file");
            return text;
        }
        text = str.MarshalUtf8().AsPtr();

        if (text is null)
        {
            Log.Warn("AssetLoading", $"FILEIO: [{filename}] Failed to allocated memory for file reading");
            return text;
        }

        Log.Info("AssetLoading", $"FILEIO: [{filename}] Text file loaded successfully");
        return text;
    }

    protected void OnReady() {
        
        var game = new FlappyPegasus.Game();
        var displayMetrics = new DisplayMetrics();
        WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
        int height = displayMetrics.HeightPixels;
        int width = displayMetrics.WidthPixels;
        Raylib.InitWindow(width, height, "NekoRay");
        try {
            var loopFunction = game.Run(new string[]{});
            while (!(Raylib.WindowShouldClose() || Program.ShouldQuit)) {
                loopFunction();
            }
        }
        catch (System.Exception e) when (!Debugger.IsAttached){
            var loopFunction = game.ErrorHandler(e);
            while (!(Raylib.WindowShouldClose() || Program.ShouldQuit)) {
                loopFunction();
            }
        }
        Raylib.CloseWindow();
    }

    [DllImport("libraylib", CallingConvention = CallingConvention.Cdecl)]
    private static extern void RaylibSetAndroidCallback(Action callback);
}