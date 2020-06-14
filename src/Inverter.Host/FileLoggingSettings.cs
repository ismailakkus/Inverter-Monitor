using Serilog;
using Serilog.Events;

namespace Inverter.Host
{
    public class FileLoggingSettings
    {
        public string Filename { get; set; } = "log.txt";
        public RollingInterval RollingInterval { get; set; } = RollingInterval.Day;
        public bool RollOnFileSizeLimit { get; set; } = true;
        public int RetainFileCountLimit { get; set; } = 31;
        public LogEventLevel MinimumLevel { get; set; } = LogEventLevel.Warning;
    }
}