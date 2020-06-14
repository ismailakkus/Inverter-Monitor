using Inverter.GoodWe;
using Inverter.Publish.Mqtt;

using Microsoft.Extensions.Configuration;

namespace Inverter.Host
{
    internal class Settings
    {
        public GoodWeSettings GoodWeSettings { get; set; } = new GoodWeSettings();
        public ServiceSettings ServiceSettings { get; set; } = new ServiceSettings();
        public MqttPublisherSettings MqttPublisherSettings { get; set; } = new MqttPublisherSettings();
        public LoggingSettings LoggerSettings { get; set; } = new LoggingSettings();

        public static Settings From(IConfiguration configuration)
        {
            var settings = new Settings();
            configuration.Bind(settings);
            return settings;
        }
    }
}