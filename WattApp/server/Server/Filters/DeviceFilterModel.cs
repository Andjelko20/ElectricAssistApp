namespace Server.Filters
{
    public class DeviceFilterModel
    {
        public long? categoryId { get; set; }
        public long? typeId { get; set; }
        public long? brandId { get; set; }
        public bool? turnOn { get; set; }
        public bool? controlability { get; set; }
    }
}
