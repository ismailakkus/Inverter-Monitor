namespace Inverter.Publish.Mqtt
{
    public class MqttPublisherSettings
    {
        public MqttConnectionSettings ConnectionSettings { get; set; } = new MqttConnectionSettings();
        public MqttPublishSettings PublishSettings { get; set; } = new MqttPublishSettings();
    }
}