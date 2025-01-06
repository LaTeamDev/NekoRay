namespace NeuroSama.Gameplay.Dialogue;

public class DialogueSetFadeBg(float ammount) : DialogueEvent {
    public override bool Skip { get; set; } = true;
    public float Ammount = ammount;
}