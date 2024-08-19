namespace NekoRay; 

public class NekoRayConf {
    public class GameMeta {
        public List<string>? Developers { get; set; }
        public string? Website { get; set; }
        public string? License { get; set; }
    }

    public class FilesystemPaths {
        public List<string> Bin { get; set; } = new();
        public List<string> Mount { get; set; } = new();
    }
    
    public string Name { get; set; } = "NekoRay";
    public GameMeta? Meta { get; set; }
    public FilesystemPaths Filesystem { get; set; } = new();

    public WindowSettings Window { get; set; } = new();
}