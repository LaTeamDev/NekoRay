using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Android.Hardware.Display;
using Android.Util;
using Android.Views;
using Java.Lang;
using NekoRay;
using ZeroElectric.Vinculum;
using Activity = Android.App.Activity;

namespace NekoRayDroid;

[MetaData("android.app.lib_name", Value = "raylib")]
[Activity(Label = "@string/app_name", MainLauncher = true)]
public class RaylibActivity : NativeActivity {
    
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
    
    protected override void OnCreate(Bundle? savedInstanceState) {
        JavaSystem.LoadLibrary("raylib");
        RaylibSetAndroidCallback(OnReady);
        Log.Debug(null, "meow");  
        base.OnCreate(savedInstanceState);
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