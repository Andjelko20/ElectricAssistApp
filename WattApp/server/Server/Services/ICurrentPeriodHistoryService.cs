using Server.DTOs;

namespace Server.Services
{
    public interface ICurrentPeriodHistoryService
    {
        public double GetUsageHistoryForDeviceFromCurrentYear(long deviceId);
        public double GetUsageHistoryForDeviceFromCurrentMonth(long deviceId);
        public double GetUsageHistoryForDeviceFromCurrentDay(long deviceId);
        public List<MonthlyEnergyConsumptionLastYear> GetUsageHistoryForDeviceFromCurrentYearByMonth(long deviceId);
    }
}
