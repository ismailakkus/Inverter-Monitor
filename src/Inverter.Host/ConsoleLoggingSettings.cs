using Serilog.Events;

namespace Inverter.Host
{
    public class ConsoleLoggingSettings
    {
        public LogEventLevel MinimumLevel { get; set; } = LogEventLevel.Warning;
    }
}