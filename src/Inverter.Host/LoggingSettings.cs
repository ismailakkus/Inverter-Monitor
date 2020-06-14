namespace Inverter.Host
{
    public class LoggingSettings
    {
        public bool EnableFileLogging { get; set; } = true;
        public bool EnableConsoleLogging { get; set; } = true;

        public FileLoggingSettings FileLoggingSettings { get; set; } = new FileLoggingSettings();
        public ConsoleLoggingSettings ConsoleLoggingSettings { get; set; } = new ConsoleLoggingSettings();
    }
}