namespace NeuroSama.Gameplay.Dialogue;

public class DialogueEntry(string name, string text) : DialogueEvent {
    public string Name = name;
    public string Text = text;
}