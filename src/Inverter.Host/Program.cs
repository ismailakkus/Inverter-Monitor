using System;
using System.IO;

using Inverter.GoodWe;
using Inverter.Interfaces;
using Inverter.Publish.Mqtt;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

namespace Inverter.Host
{
    public class Program
    {
        private const string configurationFileBasePath = "configuration";
        private const string loggingFileBasePath = "logging";
        private const string environmentConfigurationPrefix = "INVERTER_HOST_";

        public static int Main(string[] args)
        {
            ILogger logger = BuildLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch(Exception exception)
            {
                logger.Fatal(exception, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static ILogger BuildLogger(LoggingSettings settings = null)
        {
            settings ??= new LoggingSettings();

            var configuration = new LoggerConfiguration()
                                .Enrich.FromLogContext()
                                .MinimumLevel.Information()
                                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);

            if(settings.EnableFileLogging)
                configuration.WithFileLogging(loggingFileBasePath, settings.FileLoggingSettings);
            if(settings.EnableConsoleLogging)
                configuration.WithConsoleLogging(settings.ConsoleLoggingSettings);

            return Log.Logger = configuration.CreateLogger();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                     .ConfigureHostConfiguration(builder =>
                                                 {
                                                     builder.SetBasePath(Directory.GetCurrentDirectory());
                                                     builder.AddYamlFile(Path.Combine(configurationFileBasePath, "configuration.yml"), optional:true);
                                                     builder.AddYamlFile(Path.Combine(configurationFileBasePath, "configuration.yaml"), optional:true);
                                                     builder.AddJsonFile(Path.Combine(configurationFileBasePath, "configuration.json"), optional:true);
                                                     builder.AddEnvironmentVariables(prefix:environmentConfigurationPrefix);
                                                     builder.AddCommandLine(args);
                                                 })
                     .ConfigureServices((hostContext, services) =>
                                        {
                                            var settings = Settings.From(hostContext.Configuration);
                                            var logger = BuildLogger(settings.LoggerSettings);
                                            services.AddSingleton(logger);

                                            var mqttPublisherFactory = new MqttPublisherFactory(settings.MqttPublisherSettings);
                                            var mqttClient = mqttPublisherFactory.ManagedMqttClient().GetAwaiter().GetResult();

                                            services.AddSingleton(provider => GoodWeInvertersFactory.Build(settings.GoodWeSettings,
                                                                                                           new Observe {LogAuthentication = () => logger.Information("Authenticating against GoodWe api")},
                                                                                                           () => DateTimeOffset.UtcNow));
                                            services.AddTransient<IPublisher, LoggingPublisher>();
                                            services.AddSingleton(provider => mqttPublisherFactory.Build(mqttClient));
                                            services.AddHostedService(provider => new Service(provider.GetService<Inverters>(), provider.GetServices<IPublisher>(), settings.ServiceSettings));
                                        });
    }
}