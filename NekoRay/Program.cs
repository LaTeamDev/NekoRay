using System.Reflection;

namespace NekoRay; 

public class Program {
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
	
    
    public static void Init()
    {
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        GamePath = Path.GetDirectoryName(Environment.ProcessPath);
        _dllPaths.Add(Path.Join(GamePath, "\\bin\\"));
        Environment.CurrentDirectory = GamePath;
    }
	
    private static Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
    {
        var name = args.Name.Split(',')[0];
        foreach (var dllPath in _dllPaths) {
            var path = Path.Join(dllPath, name + ".dll");
            if (File.Exists(path)) return Assembly.LoadFrom(path);
        }
        return null;
    }
		
    public static void Main(string[] args) {
        Init();
        Bootstrapper.Start(args);
    }
}