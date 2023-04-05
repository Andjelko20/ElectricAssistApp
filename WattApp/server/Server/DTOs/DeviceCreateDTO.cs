namespace Server.DTOs
{
    public class DeviceCreateDTO
    {
        public long UserId { get; set; }
        /*public long DeviceCategoryId { get; set; }
        public long DeviceTypeId { get; set; }
        public long DeviceBrandId { get; set; }*/
        public long DeviceModelId { get; set; }
        public string Name { get; set; }
        public float EnergyInKwh { get; set; } = 0;
        public float StandByKwh { get; set; } = 0;
        public bool Visibility { get; set; } = false;
        public bool Controlability { get; set; } = false ;
        public bool TurnOn { get; set; } = false;
    }
}
