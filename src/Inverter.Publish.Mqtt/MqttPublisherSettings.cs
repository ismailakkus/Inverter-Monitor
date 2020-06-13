namespace Inverter.Publish.Mqtt
{
    public class MqttPublisherSettings
    {
        public MqttConnectionSettings ConnectionSettings { get; } = new MqttConnectionSettings();
        public MqttPublishSettings PublishSettings { get; } = new MqttPublishSettings();
    }
}