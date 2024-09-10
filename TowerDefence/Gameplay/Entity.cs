using NekoLib.Core;
using NekoRay.Tools;
using ZeroElectric.Vinculum;

namespace TowerDefence.Gameplay;

public class Entity : GameObject {

    [ConVariable("r_draw_entity_position")]
    [ConTags("cheat")]
    public static bool DrawEntityPosition { get; set; }

    public Entity(string name = "Entity") : base(name) {
        AddComponent<EntityRenderer>();
    }

    public float Hp = 1f;
    
    public virtual void Render() {
        if (DrawEntityPosition) Raylib.DrawText("entity", Transform.Position.X, Transform.Position.Y, 10, Raylib.BLACK);
    }
    
}