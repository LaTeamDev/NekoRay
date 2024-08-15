using System.Numerics;
namespace HotlineSPonyami.Tools;
public abstract class BaseTool
{
    public abstract string Name { get; }
    private EditorScene? _scene; protected EditorScene Scene => _scene;

    public void Initialize(EditorScene scene)
    {
        if(_scene == null) 
            _scene = scene;
    }
    public abstract void OnDraw();
    public abstract void OnUpdate();
    public abstract void OnSelect();
    public abstract void DrawGui();
}