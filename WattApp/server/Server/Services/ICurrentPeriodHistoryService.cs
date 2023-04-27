using Server.DTOs;

namespace Server.Services
{
    public interface ICurrentPeriodHistoryService
    {
        public double GetUsageHistoryForDeviceFromCurrentYear(long deviceId);
        public double GetUsageHistoryForDeviceFromCurrentMonth(long deviceId);
        public double GetUsageHistoryForDeviceFromCurrentDay(long deviceId);
        public List<MonthlyEnergyConsumptionLastYear> GetUsageHistoryForDeviceFromCurrentYearByMonth(long deviceId);
        public List<DailyEnergyConsumptionPastMonth> GetUsageHistoryForDeviceFromCurrentMonthByDay(long deviceId);
        public List<EnergyToday> GetUsageHistoryForProsumerFromCurrentDayByHour(long userId, long deviceCategoryId);
        public List<DailyEnergyConsumptionPastMonth> GetUsageHistoryForProsumerFromCurrentMonthByDay(long userId, long deviceCategoryId);
        public List<MonthlyEnergyConsumptionLastYear> GetUsageHistoryForProsumerFromCurrentYearByMonth(long userId, long deviceCategoryId);
        public double GetUsageHistoryForProsumerFromCurrentDay(long userId, long deviceCategoryId);
        public double GetUsageHistoryForProsumerFromCurrentMonth(long userId, long deviceCategoryId);
        public double GetUsageHistoryForProsumerFromCurrentYear(long userId, long deviceCategoryId);
    }
}
