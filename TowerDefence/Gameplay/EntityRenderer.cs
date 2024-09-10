using NekoLib.Core;

namespace TowerDefence.Gameplay;

public class EntityRenderer : Behaviour {
    public Entity Entity => (Entity)GameObject;

    void Render() {
        Entity.Render();
    }
}