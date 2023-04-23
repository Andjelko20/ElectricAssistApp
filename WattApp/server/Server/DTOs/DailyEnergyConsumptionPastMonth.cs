namespace Server.DTOs
{
    public class DailyEnergyConsumptionPastMonth
    {
        public int Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public double EnergyUsageResult { get; set; }
    }
}
