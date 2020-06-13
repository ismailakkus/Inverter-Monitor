namespace Inverter.GoodWe.PowerStationData
{
    internal class Energestatisticscharts
    {
        public float sum { get; set; }
        public float buy { get; set; }
        public float buyPercent { get; set; }
        public float sell { get; set; }
        public float sellPercent { get; set; }
        public float selfUseOfPv { get; set; }
        public float consumptionOfLoad { get; set; }
        public int chartsType { get; set; }
        public bool hasPv { get; set; }
        public bool hasCharge { get; set; }
        public float charge { get; set; }
        public float disCharge { get; set; }
    }
}