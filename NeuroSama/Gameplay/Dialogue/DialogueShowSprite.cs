using NekoRay;

namespace NeuroSama.Gameplay.Dialogue;

public class DialogueShowSprite(string name, string? emotion = null, DialoguePosition position = DialoguePosition.Center) : DialogueEvent {
    public string Name = name;
    public string Emotion = emotion??"neutral";
    public DialoguePosition Position = position;
    public override bool Skip { get; set; } = true;
}

public enum DialoguePosition {
    Left,
    Center,
    Right
}