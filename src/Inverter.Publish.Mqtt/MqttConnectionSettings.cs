using System;

using MQTTnet.Client.Options;

namespace Inverter.Publish.Mqtt
{
    public class MqttConnectionSettings
    {
        public TimeSpan ReconnectDelay { get; } = TimeSpan.FromSeconds(15);
        public string ClientId { get; } = "inverter";
        public string Host { get; } = "localhost";
        public int? Port { get; } = null;

        internal IMqttClientOptions AsMqttClientOptions()
            => new MqttClientOptionsBuilder().WithClientId(ClientId)
                                             .WithTcpServer(Host, Port)
                                             .WithCleanSession()
                                             .Build();
    }
}