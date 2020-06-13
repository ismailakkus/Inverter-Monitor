namespace Inverter.GoodWe.PowerStationData
{
    internal class Data
    {
        public Info info { get; set; }
        public Kpi kpi { get; set; }
        public object[] images { get; set; }
        public Weather weather { get; set; }
        public Inverter[] inverter { get; set; }
        public Hjgx hjgx { get; set; }
        public object pre_powerstation_id { get; set; }
        public object nex_powerstation_id { get; set; }
        public Homkit homKit { get; set; }
        public Smuggleinfo smuggleInfo { get; set; }
        public Powerflow powerflow { get; set; }
        public Energestatisticscharts energeStatisticsCharts { get; set; }
        public Soc soc { get; set; }
    }
}