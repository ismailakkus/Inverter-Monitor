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
    //https://github.com/DiedB/Homey-SolarPanels/blob/master/drivers/goodwe/api.js
    //https://github.com/chkr1011/MQTTnet/wiki/Client

    public class Program
    {
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
                                                     builder.AddYamlFile("configuration.yaml", optional:true);
                                                     builder.AddEnvironmentVariables(prefix:"host_");
                                                     builder.AddCommandLine(args);
                                                 })
                     .ConfigureServices((hostContext, services) =>
                                        {
                                            var settings = Settings.From(hostContext.Configuration);

                                            var mqttPublisherFactory = new MqttPublisherFactory(settings.MqttPublisherSettings);
                                            var mqttClient = mqttPublisherFactory.ManagedMqttClient().GetAwaiter().GetResult();

                                            services.AddSingleton(provider => GoodWeInvertersFactory.Build(settings.GoodWeSettings,
                                                                                                           new Observe {LogAuthentication = () => Console.WriteLine("authenticating")},
                                                                                                           () => DateTimeOffset.UtcNow));
                                            services.AddTransient<IPublisher>(_ => new ConsolePublisher());
                                            services.AddSingleton(provider => mqttPublisherFactory.Build(mqttClient));
                                            services.AddHostedService(provider => new Service(provider.GetService<Inverters>(), provider.GetServices<IPublisher>(), settings.ServiceSettings));
                                        });
    }
}