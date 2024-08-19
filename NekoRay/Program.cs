using System.Reflection;

namespace NekoRay; 

public class Program {
    private static bool _shouldQuit = false;
    public static bool ShouldQuit => _shouldQuit;
    public static void Quit() => _shouldQuit = true;
    
    public static string GamePath { get; set; }
	
    public static string DllPath { get; set; }
	
    
    public static void Init()
    {
        AppDomain currentDomain = AppDomain.CurrentDomain;
        currentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        GamePath = Path.GetDirectoryName(Environment.ProcessPath);
        DllPath = Path.Join(GamePath, "\\bin\\");
        Environment.CurrentDirectory = GamePath;
        var path = Environment.GetEnvironmentVariable("PATH");
        path = DllPath + ";" + path;
        Environment.SetEnvironmentVariable("PATH", path);
    }
	
    private static Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
    {
        var name = args.Name.Split(',')[0];
        var path = DllPath + "\\" + name + ".dll";
        if (!File.Exists(path)) return null;
        return Assembly.LoadFrom(path);
    }
		
    public static void Main(string[] args) {
        Init();
        Bootstrapper.Start(args);
    }
}