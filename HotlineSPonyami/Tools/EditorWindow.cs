using NekoLib.Core;

namespace HotlineSPonyami.Tools;

public abstract class EditorWindow : Behaviour
{
    private EditorScene? _scene; protected EditorScene Scene => _scene;
    public void Initialize(EditorScene scene)
    {
        if (_scene == null)
            _scene = scene;
    }
}