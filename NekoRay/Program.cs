using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace NekoRay; 

public static class Program {
    private static bool _shouldQuit = false;
    public static bool ShouldQuit => _shouldQuit;
    public static void Quit() => _shouldQuit = true;
    
    public static string GamePath { get; set; }

    private static List<string> _dllPaths = new();
    private static string OldPath = Environment.GetEnvironmentVariable("PATH");
    private static string NewPath => OldPath + ";" + string.Join(";", _dllPaths);

    public static void AddPath(string path) {
        _dllPaths.Add(path);
        Environment.SetEnvironmentVariable("PATH", NewPath);
    }
	#if WINDOWS
    [DllImport("user32.dll", EntryPoint = "MessageBoxW",CharSet = CharSet.Auto)]
    public static extern int MessageBox(nint hWnd, string text, string caption, long type);
    
    private static void WinCrashHandler(object sender, UnhandledExceptionEventArgs e) {
        if (e.ExceptionObject is Exception exception) {
            MessageBox(0, 
                $"NekoRay encountered an error it could not recover from:\n{exception}\n\n If you see an error about dll not found, most probably you don't have Visual C++ Redistributables installed.", "NekoRay", 0x00000010L);
        }
        else {
            MessageBox(0, 
                $"NekoRay encountered an error it could not recover from AND it is not exception.\nHow?", "NekoRay", 0x00000010L);
        }
    }
    
    public static void SetCrashHandler() {

        AppDomain.CurrentDomain.UnhandledException += WinCrashHandler;
    }
    #endif

    public static void Init() {
#if WINDOWS
        SetCrashHandler();
#endif
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        GamePath = Directory.GetCurrentDirectory();
        _dllPaths.Add(Path.Join(GamePath, "bin"));
        Environment.CurrentDirectory = GamePath;
    }
	
    private static Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
    {
        var name = args.Name.Split(',')[0];
        for (var index = _dllPaths.Count - 1; index >= 0; index--) {
            var dllPath = _dllPaths[index];
            var path = Path.Join(dllPath, name + ".dll");
            if (File.Exists(path)) return Assembly.LoadFrom(path);
        }

        return null;
    }
    
    [HandleProcessCorruptedStateExceptions] 
    public static void Main(string[] args) {
        Init();
        Bootstrapper.Start(args);
    }
}