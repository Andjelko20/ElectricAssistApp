namespace Server.DTOs
{
    public class EnergyToday
    {
        public double EnergyUsageResult { get; set; }
        public int Hour { get; set; }
        public int Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
    }
}
