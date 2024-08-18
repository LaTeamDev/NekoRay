using System.Collections.ObjectModel;
using System.Numerics;
using ImGuiNET;
using NekoLib.Filesystem;
using NekoRay;
using Tomlyn;
using Tomlyn.Model;

namespace HotlineSPonyami.Tools;

public static class UnpackedTextures
{
    private static Texture[]? _floorTextures;
    private static Texture[]? _wallTextures;

    private static void Initialize()
    {
        if (Files.FileExists("textures.toml"))
        {
            TomlTable table = Toml.ToModel(Files.GetFile("textures.toml").Read());
            TomlArray paths = (TomlArray)table["floor_textures"];
            _floorTextures = new Texture[paths.Count()];
            for (int i = 0; i < paths.Count(); i++)
            {
                string path = "textures/unpacked/floors/" + (string)paths[i];
                _floorTextures[i] = Data.GetTexture(path);
            }
            TomlArray wallPaths = (TomlArray)table["wall_textures"];
            _wallTextures = new Texture[wallPaths.Count()];
            for (int i = 0; i < wallPaths.Count(); i++)
            {
                string path = "textures/unpacked/walls/" + (string)wallPaths[i];
                _wallTextures[i] = Data.GetTexture(path);
            }
        }
        else
        {
            TomlTable table = new TomlTable();
            table["floor_textures"] = new string[] { "dev_01.png" };
            table["wall_textures"] = new string[] { "dev_wall.png" };
            Files.GetWritableFilesystem().CreateFile("textures.toml").Write(Toml.FromModel(table));
            _floorTextures = new Texture[] { Data.GetTexture("textures/notexture.png") };
            _wallTextures = new Texture[] { Data.GetTexture("textures/notexture.png") };
        }
    }

    public static ReadOnlyCollection<Texture> GetAllFloorTextures()
    {
        if (_floorTextures == null)
            Initialize();
        return _floorTextures.AsReadOnly();
    }
    
    public static Texture GetFloorTextureById(int id)
    {
        if (_floorTextures == null)
            Initialize();
        return _floorTextures[id];
    }
    public static ReadOnlyCollection<Texture> GetAllWallTextures()
    {
        if (_wallTextures == null)
            Initialize();
        return _wallTextures.AsReadOnly();
    }
    
    public static Texture GetWallTextureById(int id)
    {
        if (_wallTextures == null)
            Initialize();
        return _wallTextures[id];
    }

    public static void DrawImGuiSelector(ref byte selectedTexture, ref Texture? previewTexture, ref bool isSelecting, ReadOnlyCollection<Texture> textures)
    {
        ImGui.Text("Selected: " + selectedTexture);
        if (selectedTexture > 0 && previewTexture == null)
        {
            previewTexture = textures[selectedTexture];
        }
        if(previewTexture != null) ImGui.Image((IntPtr)previewTexture.Id, new Vector2(32, 32));
        if (ImGui.Button("Clear"))
        {
            previewTexture = null;
            selectedTexture = 0;
        }
        ImGui.SameLine();
        if (ImGui.Button("Select"))
        {
            isSelecting = true;
            ImGui.OpenPopup("Texture Selector");
        }

        if (ImGui.BeginPopupModal("Texture Selector", ref isSelecting, ImGuiWindowFlags.AlwaysAutoResize))
        {
            if (ImGui.BeginChild("Preview", new Vector2(200, 150)))
            {
                const int perLine = 4;
                int l = 0;
                int i = 0;
                foreach (var image in textures)
                {
                    if (ImGui.ImageButton("floor_tex" + i, (IntPtr)image.Id, new Vector2(32, 32)))
                    {
                        previewTexture = null;
                        selectedTexture = (byte)(i);
                        isSelecting = false;
                    }
                    l++;
                    i++;
                    if (l >= perLine)
                        l = 0;
                    else
                        ImGui.SameLine();
                }
                ImGui.EndChild();
            }
            
            if (ImGui.Button("Cancel"))
            {
                isSelecting = false;
            }
            ImGui.EndPopup();
        }
    }
}