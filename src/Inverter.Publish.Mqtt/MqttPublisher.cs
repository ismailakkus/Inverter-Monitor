using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Inverter.Interfaces;

using MQTTnet;

namespace Inverter.Publish.Mqtt
{
    internal class MqttPublisher : IPublisher
    {
        private readonly IApplicationMessagePublisher _client;
        private readonly MqttPublishSettings _settings;

        public MqttPublisher(IApplicationMessagePublisher client, MqttPublishSettings settings)
        {
            _client = client;
            _settings = settings;
        }

        public async Task Publish(Inverter inverter, Measurement measurement, CancellationToken cancellationToken)
        {
            var messages = new List<MqttApplicationMessage>
                           {
                               Create($"{_settings.RootTopic}/{inverter.Id}/temperature",
                                      measurement.Temperature.ToString(CultureInfo.InvariantCulture)),
                               Create($"{_settings.RootTopic}/{inverter.Id}/power",
                                      measurement.Power.ToString(CultureInfo.InvariantCulture)),
                               Create($"{_settings.RootTopic}/{inverter.Id}/creationDateTime",
                                      measurement.CreatedAt.ToString("o"))
                           };

            await Task.WhenAll(messages.Select(message => _client.PublishAsync(message, cancellationToken))).ConfigureAwait(false);

            static MqttApplicationMessage Create(string topic, string body)
                => new MqttApplicationMessageBuilder().WithTopic(topic)
                                                      .WithPayload(body)
                                                      .WithExactlyOnceQoS()
                                                      .WithRetainFlag()
                                                      .Build();
        }
    }
}