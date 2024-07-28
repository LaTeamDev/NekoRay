using NekoLib.Core;

namespace HotlineSPonyami.EntityTemplates;

public class TestEntityTemplate : EntityTemplate
{
    public TestEntityTemplate()
    {
        _mainTexture = Data.GetTexture("data/textures/unpacked/floors/dev_floor.png");
    }
    
    public override GameObject CreateObject()
    {
        GameObject gam = new GameObject("Test Template");
        return gam;
    }
}