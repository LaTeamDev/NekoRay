using NekoRay.Tools;
using TowerDefence.Gameplay;

namespace TowerDefence.Tools;

[CustomInspector(typeof(Entity))]
public class EntityInspector : ObjectInspector {
    private GameObjectInspector _gameObjectInspector;
    public override void Initialize() {
        base.Initialize();
        _gameObjectInspector = new GameObjectInspector();
        _gameObjectInspector.Target = Target;
        _gameObjectInspector.Initialize();
    }

    public override void DrawGui() {
        base.DrawGui();
        _gameObjectInspector.DrawGui();
    }

    public override void Dispose() {
        base.Dispose();
        _gameObjectInspector.Dispose();
    }
}