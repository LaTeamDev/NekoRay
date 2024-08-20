using Serilog;
using Serilog.Configuration;

namespace NekoRay.Tools; 

public static class ConsoleSinkExtensions
{
    public static LoggerConfiguration GameConsole(
        this LoggerSinkConfiguration loggerConfiguration,
        IFormatProvider formatProvider = null)
    {
        return loggerConfiguration.Sink(new ConsoleSink(formatProvider));
    }
}