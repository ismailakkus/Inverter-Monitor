using System.Text.Json;

namespace Inverter.GoodWe.PowerStationData
{
    internal class PowerStationResponse
    {
        public string language { get; set; }
        public string[] function { get; set; }
        public bool hasError { get; set; }
        public string msg { get; set; }
        public string code { get; set; }
        public Data data { get; set; }
        public Components components { get; set; }

        public static PowerStationResponse From(string response)
            => JsonSerializer.Deserialize<PowerStationResponse>(response);
    }
}