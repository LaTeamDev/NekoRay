using System.Collections.ObjectModel;
using NekoRay;
using Tomlyn;
using Tomlyn.Model;

namespace HotlineSPonyami.Tools;

public static class UnpackedTextures
{
    private static Texture[]? _floorTextures;

    private static void Initialize()
    {
        if (File.Exists("data/textures.toml"))
        {
            TomlTable table = Toml.ToModel(File.ReadAllText("data/textures.toml"));
            TomlArray paths = (TomlArray)table["floor_textures"];
            _floorTextures = new Texture[paths.Count()];
            for (int i = 0; i < paths.Count(); i++)
            {
                string path = "data/textures/unpacked/floors/" + (string)paths[i];
                _floorTextures[i] = Texture.Load(path);
            }
        }
        else
        {
            TomlTable table = new TomlTable();
            table["floor_textures"] = new string[] { "dev_01.png" };
            File.WriteAllText("data/textures.toml",Toml.FromModel(table));
            _floorTextures = new Texture[] { Data.GetTexture("data/textures/notexture.png") };
        }
    }

    public static ReadOnlyCollection<Texture> GetAllTextures()
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
}