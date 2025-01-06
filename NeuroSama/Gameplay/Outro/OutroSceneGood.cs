using NeuroSama.Gameplay.Dialogue;
using NeuroSama.Gameplay.Intro;
using NeuroSama.Gameplay.MainMenu;
using NeuroSama.Gameplay.Wander;

namespace NeuroSama.Gameplay;

public class OutroSceneGood : Intro1Scene {
    protected override void SpawnBackground() { }

    public override void ShowDialogue() {
        DialogueController.AddStub();
        DialogueController.Add("GOOD ENDING", "UNLOCKED");
        DialogueController.Add("VanderCat", "Thanks for playing this little something of a game");
        DialogueController.Add("VanderCat", "i did end up coding it up myself, so i could not finish much");
        DialogueController.Add("VanderCat", "we tried our best, it is our first ever finished jam");
        DialogueController.Add("VanderCat", "<3");
        DialogueController.Add("Umba_Huyumba", "It is sad that we could not finish it");
        DialogueController.Add("Umba_Huyumba", "But i'm still glad you played this");
        DialogueController.Add("Umba_Huyumba", "Thank you, idk, see you? or something");
        DialogueController.ChangeScene(new WanderScene());
    }
}