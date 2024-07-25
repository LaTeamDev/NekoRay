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

    public WindowSettings Window { get; set; } = new();

    public bool Fullscreen { get; set; } = false;

    public bool Vsync { get; set; } = false;

    public bool Msaa { get; set; } = false;

    public uint GetFlags() {
        uint flags = 0;
        if (Vsync) flags |= (int) ConfigFlags.FLAG_VSYNC_HINT;
        if (Msaa) flags |= (int) ConfigFlags.FLAG_MSAA_4X_HINT;
        if (Window.Resizable) flags |= (int) ConfigFlags.FLAG_WINDOW_RESIZABLE;
        if (Window.Undecorated) flags |= (int) ConfigFlags.FLAG_WINDOW_UNDECORATED;
        if (Window.Transparent) flags |= (int) ConfigFlags.FLAG_WINDOW_TRANSPARENT;
        if (Window.Hidden) flags |= (int) ConfigFlags.FLAG_WINDOW_HIDDEN;
        if (Window.AlwaysRun) flags |= (int) ConfigFlags.FLAG_WINDOW_ALWAYS_RUN;
        if (Window.Minimized) flags |= (int) ConfigFlags.FLAG_WINDOW_MINIMIZED;
        if (Window.Maximized) flags |= (int) ConfigFlags.FLAG_WINDOW_MAXIMIZED;
        if (Window.Unfocused) flags |= (int) ConfigFlags.FLAG_WINDOW_UNFOCUSED;
        if (Window.Topmost) flags |= (int) ConfigFlags.FLAG_WINDOW_TOPMOST;
        if (Window.HighDpi) flags |= (int) ConfigFlags.FLAG_WINDOW_HIGHDPI;
        if (Window.MousePassthrough) flags |= (int) ConfigFlags.FLAG_WINDOW_MOUSE_PASSTHROUGH;
        if (Window.Interlaced) flags |= (int) ConfigFlags.FLAG_INTERLACED_HINT;
        return flags;
    }

}