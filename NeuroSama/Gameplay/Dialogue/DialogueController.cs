using NekoLib.Scenes;
using NekoRay.Tools;
using SoLoud;

namespace NeuroSama.Gameplay.Dialogue;

public static class DialogueController {

    public static DialogueTracker DialogueObservable = new();

    [ConCommand("dg_msg")]
    public static void Add(string name, params string[] text) {
        Add(name, string.Join(' ', text));
    }
    
    public static void Add(string name, string text) {
        var entry = new DialogueEntry(name, text);
        DialogueObservable.Notify(entry);
    }
    
    [ConCommand("dg_next")]
    public static void Next() {
        DialogueObservable.Notify(new DialogueNext());
    }
    
    public static void AddImage(string name, string? emotion = null, DialoguePosition position = DialoguePosition.Center) {
        DialogueObservable.Notify(new DialogueShowSprite(name, emotion, position));
    }
    public static void RemoveImage(DialoguePosition position = DialoguePosition.Center) {
        DialogueObservable.Notify(new DialogueHideSprite(position));
    }
    
    [ConCommand("dg_image")]
    public static void AddImage(string name, string? emotion = null, string position = "Center") {
        Enum.TryParse(position, true, out DialoguePosition pos);
        AddImage(name, emotion, pos);
    }
    [ConCommand("dg_unimage")]
    public static void RemoveImage(string position = "Center") {
        Enum.TryParse(position, true, out DialoguePosition pos);
        RemoveImage(pos);
    }

    public static void AddStub() {
        DialogueObservable.Notify(new DialogueStub());
    }

    public static void ChangeScene(IScene scene) {
        DialogueObservable.Notify(new DialogueChangeScene(scene));
    }
    
    public static void PlaySound(AudioSource sound) {
        DialogueObservable.Notify(new DialoguePlaySound(sound));
    }

    public static void Fade(float f) {
        DialogueObservable.Notify(new DialogueSetFadeBg(f));
    }
    public static void ToggleBox(bool state) {
        DialogueObservable.Notify(new DialogueToggleUpper(state));
    }
}