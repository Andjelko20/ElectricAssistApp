namespace Server.DTOs
{
    public class DeviceCreateDTO
    {
        public long UserId { get; set; }
        public long DeviceCategoryId { get; set; }
        public long DeviceTypeId { get; set; }
        public long DeviceBrandId { get; set; }
        public long DeviceModelId { get; set; }
        public string Name { get; set; }
        public float EnergyInKwh { get; set; }
        public float StandByKwh { get; set; }
        public bool Visibility { get; set; }
        public bool Controlability { get; set; }
        public bool TurnOn { get;  set; }
    }
}
