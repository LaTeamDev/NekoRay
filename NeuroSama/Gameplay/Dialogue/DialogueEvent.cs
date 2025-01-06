namespace NeuroSama.Gameplay.Dialogue;

public abstract class DialogueEvent {
    public virtual bool Skip {
        get => false;
        set => throw new NotSupportedException();
    }
}