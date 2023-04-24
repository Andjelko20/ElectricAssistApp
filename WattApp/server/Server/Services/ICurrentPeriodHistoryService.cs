namespace Server.Services
{
    public interface ICurrentPeriodHistoryService
    {
        public double GetUsageHistoryForDeviceFromCurrentYear(long deviceId);
        public double GetUsageHistoryForDeviceFromCurrentMonth(long deviceId);
    }
}
