namespace HotlineSPonyami.Tools;

public interface IBinarySavable
{
    public void Save(BinaryWriter writer);
    public void Load(BinaryReader reader);
}