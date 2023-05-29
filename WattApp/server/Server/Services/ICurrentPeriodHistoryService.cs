using Server.DTOs;

namespace Server.Services
{
    public interface ICurrentPeriodHistoryService
    {
        public double GetUsageHistoryForProsumerFromCurrentDay(long userId, long deviceCategoryId);
        public double GetUsageHistoryForProsumerFromCurrentMonth(long userId, long deviceCategoryId);
        public double GetUsageHistoryForProsumerFromCurrentYear(long userId, long deviceCategoryId);
        public List<EnergyToday> GetProsumerTodayByHourEnergyUsagePagination(long userId, long deviceCategoryId, int pageNumber, int itemsPerPage);
        public List<DailyEnergyConsumptionPastMonth> GetProsumerMonthByDayEnergyUsagePagination(long userId, long deviceCategoryId, int pageNumber, int itemsPerPage);
        public List<DailyEnergyConsumptionPastMonth> GetDeviceMonthByDayEnergyUsagePagination(long deviceId, int pageNumber, int itemsPerPage);
    }
}
