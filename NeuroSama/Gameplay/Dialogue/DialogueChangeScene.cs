using NekoLib.Scenes;

namespace NeuroSama.Gameplay.Dialogue;

public class DialogueChangeScene(IScene scene) : DialogueEvent {
    public IScene Scene = scene;
}