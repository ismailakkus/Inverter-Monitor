namespace Inverter.GoodWe.Systems
{
    internal class Datum
    {
        public string powerstation_id { get; set; }
        public string stationname { get; set; }
        public string first_letter { get; set; }
        public string adcode { get; set; }
        public string location { get; set; }
        public int status { get; set; }
        public float pac { get; set; }
        public float capacity { get; set; }
        public float eday { get; set; }
        public float emonth { get; set; }
        public float eday_income { get; set; }
        public float etotal { get; set; }
        public string powerstation_type { get; set; }
        public object pre_org_id { get; set; }
        public object org_id { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public float pac_kw { get; set; }
        public float to_hour { get; set; }
        public Weather weather { get; set; }
        public string currency { get; set; }
        public float yield_rate { get; set; }
        public bool is_stored { get; set; }
    }
}