namespace Inverter.GoodWe.Login
{
    internal class Data
    {
        public string uid { get; set; }
        public long timestamp { get; set; }
        public string token { get; set; }
        public string client { get; set; }
        public string version { get; set; }
        public string language { get; set; }

        public static Data CreateEmpty
            => new Data
               {
                   client = "ios",
                   version = "v2.3.0",
                   language = "en"
               };
    }
}