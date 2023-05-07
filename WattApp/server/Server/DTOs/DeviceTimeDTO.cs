namespace Server.DTOs
{
    public class DeviceTimeDTO
    {
        public int StartDay { get; set; }
        public string StartMonth { get; set; }
        public int StartYear { get; set; }
        public int EndDay { get; set; }
        public string EndMonth { get; set; }
        public int EndYear { get; set; }
        public double SecondsWorked { get; set; }
        public double MinutesWorked { get; set; }
        public double HoursWorked { get; set; }
    }
}
