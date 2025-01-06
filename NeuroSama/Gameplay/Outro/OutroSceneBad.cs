using NeuroSama.Gameplay.Dialogue;
using NeuroSama.Gameplay.Intro;
using NeuroSama.Gameplay.MainMenu;
using NeuroSama.Gameplay.Wander;

namespace NeuroSama.Gameplay;

public class OutroSceneBad : Intro1Scene {
    protected override void SpawnBackground() { }

    public override void ShowDialogue() {
        DialogueController.AddStub();
        DialogueController.Add("Neuro", "Did i really sung that?");
        DialogueController.Add("Neuro", "As a first song?");
        DialogueController.Add("Neuro", "...");
        DialogueController.Add("Neuro", "You lied, right?");
        DialogueController.ChangeScene(new MenuScene());
    }
}