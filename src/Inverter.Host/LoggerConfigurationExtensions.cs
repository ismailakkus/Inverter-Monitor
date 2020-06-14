using System.IO;

using Serilog;

namespace Inverter.Host
{
    internal static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration WithFileLogging(this LoggerConfiguration configuration,
                                                          string pathBase,
                                                          FileLoggingSettings settings)
        {
            return configuration.WriteTo.File(Path.Combine(pathBase, settings.Filename),
                                              rollingInterval:settings.RollingInterval,
                                              rollOnFileSizeLimit:settings.RollOnFileSizeLimit,
                                              retainedFileCountLimit:settings.RetainFileCountLimit,
                                              restrictedToMinimumLevel: settings.MinimumLevel);
        }

        public static LoggerConfiguration WithConsoleLogging(this LoggerConfiguration configuration,
                                                             ConsoleLoggingSettings settings)
        {
            return configuration.WriteTo.Console(restrictedToMinimumLevel: settings.MinimumLevel);
        }
    }
}