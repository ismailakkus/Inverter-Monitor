using System.Text.Json;

namespace Inverter.GoodWe.Login
{
    internal class LoginResponse
    {
        public bool hasError { get; set; }
        public int code { get; set; }
        public string msg { get; set; }
        public Data data { get; set; }
        public Components components { get; set; }
        public string api { get; set; }

        public static LoginResponse From(string response)
            => JsonSerializer.Deserialize<LoginResponse>(response);
    }
}