using System.Numerics;
using HotlineSPonyami.EntityTemplates;
using HotlineSPonyami.Tools;
using ImGuiNET;
using NekoLib.Core;
using ZeroElectric.Vinculum;
using Texture = NekoRay.Texture;

namespace HotlineSPonyami;

public abstract class EntityTemplate : IBinarySavable
{
    public Vector2 Position;
    protected Texture? _mainTexture;

    public static readonly System.Type[] Types = new[]
    {
        typeof(TestEntityTemplate)
    };

    public virtual void Save(BinaryWriter writer)
    {
        writer.Write(Position.X);
        writer.Write(Position.Y);
    }

    public virtual void Load(BinaryReader reader)
    {
        Position.X = reader.ReadSingle();
        Position.Y = reader.ReadSingle();
    }

    public virtual void OnDrawGui()
    {
        ImGui.InputFloat2("Position", ref Position);
    }
    public abstract GameObject CreateObject();

    public virtual void Draw()
    {
        if(_mainTexture == null) return;
        Rectangle source = new Rectangle(0, 0, _mainTexture.Width, _mainTexture.Height);
        Rectangle destination = new Rectangle(Position.X, Position.Y, _mainTexture.Width, _mainTexture.Height);
        _mainTexture.Draw(source, destination, Vector2.One / 2f, 0f, Raylib.WHITE);
    }

    public static EntityTemplate CreateByName(string name)
    {
        var type = Types.FirstOrDefault(t => t.Name == name);
        if (type != null)
        {
            object? obj = Activator.CreateInstance(type);
            if (obj is EntityTemplate template)
            {
                return template;
            }
        }
        return null;
    }
}