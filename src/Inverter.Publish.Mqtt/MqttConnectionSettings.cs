using System;

using MQTTnet.Client.Options;

namespace Inverter.Publish.Mqtt
{
    public class MqttConnectionSettings
    {
        public TimeSpan ReconnectDelay { get; set; } = TimeSpan.FromSeconds(15);
        public string ClientId { get; set; } = "inverter";
        public string Host { get; set; } = "localhost";
        public int? Port { get; set; } = null;

        internal IMqttClientOptions AsMqttClientOptions()
            => new MqttClientOptionsBuilder().WithClientId(ClientId)
                                             .WithTcpServer(Host, Port)
                                             .WithCleanSession()
                                             .Build();
    }
}