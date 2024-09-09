using NekoLib.Core;
using NekoRay.Tools;
using ZeroElectric.Vinculum;

namespace TowerDefence.Gameplay;

public class Entity : GameObject {

    [ConVariable("r_draw_entity_position")]
    [ConTags("cheat")]
    public static bool DrawEntityPosition { get; set; }

    public float Hp = 1f;

    public override void Draw() {
        base.Draw();
        if (DrawEntityPosition) Raylib.DrawText("player", Transform.Position.X, Transform.Position.Y, 10, Raylib.BLACK);
    }
    
}