using System.Text.Json;

namespace Inverter.GoodWe.Systems
{
    internal class SystemResponse
    {
        public bool hasError { get; set; }
        public int code { get; set; }
        public string msg { get; set; }
        public Datum[] data { get; set; }
        public Components components { get; set; }

        public static SystemResponse From(string response)
            => JsonSerializer.Deserialize<SystemResponse>(response);
    }
}