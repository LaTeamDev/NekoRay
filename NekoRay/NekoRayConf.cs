namespace NekoRay; 

public class NekoRayConf {
    public string? Identity { get; set; } = "game";
    public int Width { get; set; } = 800;
    public int Height { get; set; } = 600;

    public class WindowSettings {
        public bool Resizable { get; set; } = false;

        public bool Undecorated { get; set; } = false;

        public bool Transparent { get; set; } = false;

        public bool Hidden { get; set; } = false;

        public bool AlwaysRun { get; set; } = false;

        public bool Minimized { get; set; } = false;

        public bool Maximized { get; set; } = false;

        public bool Unfocused { get; set; } = false;

        public bool Topmost { get; set; } = false;

        public bool HighDpi { get; set; } = false;

        public bool MousePassthrough { get; set; } = false;

        public bool Interlaced { get; set; } = false;
    }

    public WindowSettings Window;

    public bool Fullscreen { get; set; } = false;

    public bool Vsync { get; set; } = false;

    public bool Msaa { get; set; } = false;

    public uint GetFlags() {
        uint flags = 0;
        if (Vsync) flags |= (int)ConfigFlags.FLAG_VSYNC_HINT;
        if (Msaa) flags |= (int)ConfigFlags.FLAG_MSAA_4X_HINT;
        return flags;
        /* TODO:
        if (Window.Resizable) flags |= (int)
        undecorated = false
        transparent = false
        hidden = false
        alwaysRun = false
        minimized = false
        maximized = false*/
    }

}