namespace NeuroSama.Gameplay.Dialogue;

public class DialogueHideSprite(DialoguePosition position) : DialogueEvent {
    public DialoguePosition Position = position;
    public override bool Skip { get; set; } = true;
}