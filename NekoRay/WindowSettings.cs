using NekoLib.Filesystem;
using Serilog;
using Tomlyn;

namespace NekoRay; 

public class WindowSettings {
    private static WindowSettings? _instance;
    public static WindowSettings Instance {
        get {
            if (_instance is not null) return _instance;
            if (Files.FileExists("cfg/video.toml"))
                _instance = Toml.ToModel<WindowSettings>(Files.GetFile("cfg/video.toml").Read());
            else {
                if (Files.FileExists("cfg/video.default.toml"))
                    _instance = Toml.ToModel<WindowSettings>(Files.GetFile("cfg/video.default.toml").Read());
                else
                    _instance = new();
                Save();
            }
            return _instance;
        }
    }

    public static void Save() {
        try {
            Files.GetWritableFilesystem().CreateFile("cfg/video.toml").Write(Toml.FromModel(Instance));
            Log.Verbose("Successfully saved video settings");
        }
        catch (Exception e) {
            Log.Error("Failed to save video settings");
            Log.Verbose(e, "Failed to save video settings");
            throw;
        }
    }
    
    public bool Resizable { get; set; } = false;

    public bool Undecorated { get; set; } = false;

    /// <summary>
    /// Requires restart
    /// </summary>
    public bool Transparent { get; set; } = false;

    public bool Hidden { get; set; } = false;

    public bool AlwaysRun { get; set; } = false;

    public bool Minimized { get; set; } = false;

    public bool Maximized { get; set; } = false;

    public bool Unfocused { get; set; } = false;

    public bool Topmost { get; set; } = false;

    /// <summary>
    /// Requires restart
    /// </summary>
    public bool HighDpi { get; set; } = false;

    public bool MousePassthrough { get; set; } = false;

    public bool Interlaced { get; set; } = false;
    
    public bool Fullscreen { get; set; } = false;
    
    public bool Vsync { get; set; } = false;
    
    /// <summary>
    /// Requires restart
    /// </summary>
    public bool Msaa { get; set; } = false;

    public int Width { get; set; } = 800;
    public int Height { get; set; } = 600;
    
    public uint GetFlags() {
        uint flags = 0;
        if (Vsync) flags |= (int) ConfigFlags.FLAG_VSYNC_HINT;
        if (Msaa) flags |= (int) ConfigFlags.FLAG_MSAA_4X_HINT;
        if (Fullscreen) flags |= (int) ConfigFlags.FLAG_FULLSCREEN_MODE;
        if (Resizable) flags |= (int) ConfigFlags.FLAG_WINDOW_RESIZABLE;
        if (Undecorated) flags |= (int) ConfigFlags.FLAG_WINDOW_UNDECORATED;
        if (Transparent) flags |= (int) ConfigFlags.FLAG_WINDOW_TRANSPARENT;
        if (Hidden) flags |= (int) ConfigFlags.FLAG_WINDOW_HIDDEN;
        if (AlwaysRun) flags |= (int) ConfigFlags.FLAG_WINDOW_ALWAYS_RUN;
        if (Minimized) flags |= (int) ConfigFlags.FLAG_WINDOW_MINIMIZED;
        if (Maximized) flags |= (int) ConfigFlags.FLAG_WINDOW_MAXIMIZED;
        if (Unfocused) flags |= (int) ConfigFlags.FLAG_WINDOW_UNFOCUSED;
        if (Topmost) flags |= (int) ConfigFlags.FLAG_WINDOW_TOPMOST;
        if (HighDpi) flags |= (int) ConfigFlags.FLAG_WINDOW_HIGHDPI;
        if (MousePassthrough) flags |= (int) ConfigFlags.FLAG_WINDOW_MOUSE_PASSTHROUGH;
        if (Interlaced) flags |= (int) ConfigFlags.FLAG_INTERLACED_HINT;
        return flags;
    }
}