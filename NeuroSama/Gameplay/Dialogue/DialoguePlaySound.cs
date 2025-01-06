using SoLoud;

namespace NeuroSama.Gameplay.Dialogue;

public class DialoguePlaySound(AudioSource audioSource) : DialogueEvent {
    public override bool Skip { get; set; } = true;
    public AudioSource AudioSource = audioSource;
}