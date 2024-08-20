using Serilog.Core;
using Serilog.Events;

namespace NekoRay.Tools; 

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
        Console.Log(message);
    }
}