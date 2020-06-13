using Inverter.GoodWe;
using Inverter.Publish.Mqtt;

using Microsoft.Extensions.Configuration;

namespace Inverter.Host
{
    internal class Settings
    {
        public GoodWeSettings GoodWeSettings => new GoodWeSettings();
        public ServiceSettings ServiceSettings => new ServiceSettings();
        public MqttPublisherSettings MqttPublisherSettings => new MqttPublisherSettings();

        public static Settings From(IConfiguration configuration)
        {
            var settings = new Settings();
            configuration.Bind(settings);
            return settings;
        }
    }
}