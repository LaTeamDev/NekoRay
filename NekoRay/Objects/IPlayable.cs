namespace NekoRay; 

public interface IPlayable : IDisposable {
    public void Play();
    public void Stop();
    public void Pause();
    public void Resume();
    
    public virtual void Update() {} 

    public virtual void Seek(float seconds) { }
    public float Volume { get; set; }
    public float Pitch { get; set; }
    public float Pan { get; set; }
    public float? Length { get; }
    public float? TimePlayed { get; }
    public bool IsReady { get; }
    public bool IsPlaying { get; }
    public bool Loop { get; set; }
}