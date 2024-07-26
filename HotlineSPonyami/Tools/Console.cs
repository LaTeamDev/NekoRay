using System.Numerics;
using ImGuiNET;
using NekoLib.Core;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using ZeroElectric.Vinculum;
using Shader = NekoRay.Shader;
using Texture = NekoRay.Texture;

namespace HotlineSPonyami.Tools; 

public class Console : Behaviour {
    private static Console? _theConsole;
    public static Console? TheConsole => _theConsole;
    public Texture Texture;
    public int MaxMessageCount = 256;

    private Queue<string> _messageLog = new();


    void Awake() {
        _theConsole = this;
    }

    public void Log(string message) {
        while (_messageLog.Count > MaxMessageCount) {
            _messageLog.Dequeue();
        }
        _messageLog.Enqueue(message);
    }


    void DrawGui() {
        var opened = Enabled;
        ImGui.ShowDemoWindow();
        /*if (ImGui.Begin("Console", ref opened)) {
            if (ImGui.BeginChild("MessageLog", new Vector2(ImGui.GetWindowWidth(), ImGui.GetWindowHeight()))) {
                foreach (var message in _messageLog) {
                    ImGui.Text(message);
                }
            }
        }
        ImGui.End();*/
        if (Enabled != opened) {
            //Enabled = opened;
        }
    }
}

public class ConsoleSink : ILogEventSink
{
    private readonly IFormatProvider _formatProvider;

    public ConsoleSink(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(_formatProvider);
        Console.TheConsole?.Log(message);
    }
}
public static class ConsoleSinkExtensions
{
    public static LoggerConfiguration GameConsole(
        this LoggerSinkConfiguration loggerConfiguration,
        IFormatProvider formatProvider = null)
    {
        return loggerConfiguration.Sink(new ConsoleSink(formatProvider));
    }
}