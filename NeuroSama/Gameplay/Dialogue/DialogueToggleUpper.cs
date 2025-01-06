namespace NeuroSama.Gameplay.Dialogue;

public class DialogueToggleUpper(bool state) : DialogueEvent {
    public override bool Skip { get; set; } = true;
    public bool State;
}