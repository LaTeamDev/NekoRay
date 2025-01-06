using NekoRay;
using ZeroElectric.Vinculum.Extensions;

namespace NeuroSama.Gameplay.Dialogue;

public class DialogueTracker : IObservable<DialogueEvent> {
    private List<IObserver<DialogueEvent>> Observers = new();

    public IDisposable Subscribe(IObserver<DialogueEvent> observer) {
        Observers.Add(observer);
        return new AttachMode(() => Unsubscribe(observer));
    }
    public void Unsubscribe(IObserver<DialogueEvent> observer) => Observers.Remove(observer);

    public void Notify(DialogueEvent entry) {
        foreach (var observer in Observers) {
            observer.OnNext(entry);
        }
    }
}