namespace NekoRay.Tools; 

public interface IPoppableConsoleToken : IConsoleToken {
    public bool Pop { get; set; }
}