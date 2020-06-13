namespace Inverter.GoodWe.PowerStationData
{
    internal class Info
    {
        public string powerstation_id { get; set; }
        public string time { get; set; }
        public string date_format { get; set; }
        public string date_format_ym { get; set; }
        public string stationname { get; set; }
        public string address { get; set; }
        public object owner_name { get; set; }
        public object owner_phone { get; set; }
        public string owner_email { get; set; }
        public float battery_capacity { get; set; }
        public string turnon_time { get; set; }
        public string create_time { get; set; }
        public float capacity { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public string powerstation_type { get; set; }
        public int status { get; set; }
        public bool is_stored { get; set; }
        public bool is_powerflow { get; set; }
        public int charts_type { get; set; }
        public bool has_pv { get; set; }
        public bool has_statistics_charts { get; set; }
        public bool only_bps { get; set; }
        public bool only_bpu { get; set; }
        public float time_span { get; set; }
        public string pr_value { get; set; }
    }
}