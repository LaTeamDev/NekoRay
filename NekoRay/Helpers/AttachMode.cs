namespace NekoRay; 

public class AttachMode : IDisposable {
    private Action OnDetach;
    public AttachMode(Action onDetach) {
        OnDetach = onDetach;
    }
        
    public void Dispose() {
        OnDetach();
    }
}