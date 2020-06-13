namespace Inverter.GoodWe.PowerStationData
{
    internal class Heweather6
    {
        public Daily_Forecast[] daily_forecast { get; set; }
        public Basic basic { get; set; }
        public Update update { get; set; }
        public string status { get; set; }
    }
}