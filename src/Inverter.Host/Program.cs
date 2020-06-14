using System;
using System.IO;
using System.Threading.Tasks;

using Inverter.GoodWe;
using Inverter.Interfaces;
using Inverter.Publish.Mqtt;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Inverter.Host
{
    public class Program
    {
        private const string configurationFileBasePath = "configuration";
        private const string environmentConfigurationPrefix = "INVERTER_HOST_";

        public static Task Main(string[] args)
        {
            return CreateHostBuilder(args).Build()
                                          .RunAsync();
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

                                            var mqttPublisherFactory = new MqttPublisherFactory(settings.MqttPublisherSettings);
                                            var mqttClient = mqttPublisherFactory.ManagedMqttClient().GetAwaiter().GetResult();

                                            services.AddSingleton(provider => GoodWeInvertersFactory.Build(settings.GoodWeSettings,
                                                                                                           new Observe(),
                                                                                                           () => DateTimeOffset.UtcNow));
                                            services.AddTransient<IPublisher>(_ => new ConsolePublisher());
                                            services.AddSingleton(provider => mqttPublisherFactory.Build(mqttClient));
                                            services.AddHostedService(provider => new Service(provider.GetService<Inverters>(), provider.GetServices<IPublisher>(), settings.ServiceSettings));
                                        });
    }
}