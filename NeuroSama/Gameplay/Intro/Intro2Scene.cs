using System.Numerics;
using NekoLib.Core;
using NekoRay;
using NeuroSama.Gameplay.Dialogue;
using NeuroSama.Gameplay.MiniGame;
using NeuroSama.Gameplay.Wander;

namespace NeuroSama.Gameplay.Intro;

public class Intro2Scene : Intro1Scene {

    protected override void SpawnBackground() {
        var background = new GameObject("background").AddComponent<SpriteRenderer2D>();
        background.Sprite = Data.GetSprite("textures/location1.png");
        background.Transform.Position = new Vector3(-1f, -24f, 0f);
        background.ProportionallyScaleByHeight(720);
        background.Origin = new Vector2(0, 0.5f);
        background.Transform.Position = new Vector3(-640f, 0f, 0f);
    }
    
    public override void ShowDialogue() {
        DialogueController.AddStub();
        DialogueController.AddImage("neuro", "neutral", DialoguePosition.Left);
        DialogueController.Add("Neuro", "Vedal.");
        DialogueController.AddImage("vedal", "neutral", DialoguePosition.Right);
        DialogueController.Add("Vedal", "yes?");
        DialogueController.Add("Neuro", "If you say i can't have a childhood because i am an AI...");
        DialogueController.Add("Neuro", "...What could it be?");
        DialogueController.Add("Vedal", "well..");
        DialogueController.Add("Vedal", "since you are my creation...");
        DialogueController.Add("Vedal", "you would be definitely a small turtle");
        DialogueController.Add("Vedal", "with a childhood typical for small turtles");
        DialogueController.Add("Neuro", "This does indeed sound logical.");
        DialogueController.ChangeScene(new WanderScene());
    }
}