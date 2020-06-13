using System.Threading.Tasks;

using Inverter.Interfaces;

using MQTTnet;
using MQTTnet.Extensions.ManagedClient;

namespace Inverter.Publish.Mqtt
{
    public class MqttPublisherFactory
    {
        private readonly MqttPublisherSettings _settings;

        public MqttPublisherFactory(MqttPublisherSettings settings)
        {
            _settings = settings;
        }

        public IPublisher Build(IManagedMqttClient client)
            => new MqttPublisher(client, _settings.PublishSettings);

        public async Task<IManagedMqttClient> ManagedMqttClient()
        {
            var options = new ManagedMqttClientOptionsBuilder()
                          .WithAutoReconnectDelay(_settings.ConnectionSettings.ReconnectDelay)
                          .WithClientOptions(_settings.ConnectionSettings.AsMqttClientOptions())
                          .Build();

            var client = new MqttFactory().CreateManagedMqttClient();
            await client.StartAsync(options).ConfigureAwait(false);

            return client;
        }
    }
}