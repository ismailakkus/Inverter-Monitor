namespace Inverter.GoodWe.PowerStationData
{
    internal class Powerflow
    {
        public string pv { get; set; }
        public int pvStatus { get; set; }
        public string bettery { get; set; }
        public int betteryStatus { get; set; }
        public object betteryStatusStr { get; set; }
        public string load { get; set; }
        public int loadStatus { get; set; }
        public string grid { get; set; }
        public int soc { get; set; }
        public string socText { get; set; }
        public bool hasEquipment { get; set; }
        public int gridStatus { get; set; }
        public bool isHomKit { get; set; }
        public bool isBpuAndInverterNoBattery { get; set; }
    }
}