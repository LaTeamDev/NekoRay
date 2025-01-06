using NekoLib.Core;
using NekoLib.Filesystem;
using NekoLib.Scenes;
using NekoRay;
using Serilog;
using ZeroElectric.Vinculum;
using Font = ZeroElectric.Vinculum.Font;

namespace NeuroSama;

public class Preloader(IScene next) : IScene {
    private static HashSet<string> GetFilesFlatInDirectory(string dir) {
        var files = new HashSet<string>();
        
        foreach(var directory in Files.ListDirectories(dir)) {
            files.UnionWith(GetFilesFlatInDirectory(directory));
        }
        
        foreach (var file in Files.ListFiles(dir)) {
             files.Add(file);
        }

        return files;
    }

    public string Name => "Preloader";
    public bool DestroyOnLoad => true;
    public int Index { get; set; }
    public List<GameObject> GameObjects => new();
    public List<string> Textures;
    public List<string> Fonts;
    public string LastLoaded;
    private bool texturesLoaded = false;
    private int idx;
    public void Initialize() {
        Textures = GetFilesFlatInDirectory("textures").ToList();
        Fonts = GetFilesFlatInDirectory("fonts").ToList();
    }

    public void Update() {
        if (!texturesLoaded) {
            LastLoaded = Textures[idx++];
            Data.GetTexture(LastLoaded);
            if (idx >= Textures.Count) {
                texturesLoaded = true;
                idx = 0;
                return;
            }
            return;
        }
        LastLoaded = Fonts[idx++];
        Data.GetFont(LastLoaded);
        if (idx >= Fonts.Count) return;
        SceneManager.LoadScene(next);
    }

    public void Draw() {
        Raylib.DrawText("Loaded "+LastLoaded+" ", 0, 0, 16, Raylib.WHITE);
    }
}