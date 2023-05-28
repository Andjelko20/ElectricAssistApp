using Server.DTOs;
using Server.Models;
using Server.Models.DropDowns.Devices;

namespace Server.Services
{
    public interface IHistoryService
    {
        public List<MonthlyEnergyConsumptionLastYear> GetMonthlyEnergyUsage(long deviceId);
        public List<DailyEnergyConsumptionPastMonth> GetDailyEnergyUsageForPastWeek(long deviceId);
        public double GetUserEnergyConsumptionForPastMonth(long userId, long deviceCategoryId);

        // ZA PROSLEDJEN ID KORISNIKA POTROSNJA ZA GRAFIKE
        public List<MonthlyEnergyConsumptionLastYear> GetMonthlyEnergyUsageForPastYear(long userId, long deviceCategoryId);
        public List<DailyEnergyConsumptionPastMonth> UserHistoryForThePastWeek(long userId, long deviceCategoryId);
        public double GetUsageHistoryForDeviceToday(long deviceId);
        // One year
        public List<MonthlyEnergyConsumptionLastYear> CityHistoryForYearByMonth(long cityId, long deviceCategoryId, int yearNumber);
        public List<MonthlyEnergyConsumptionLastYear> SettlementHistoryForYearByMonth(long settlementId, long deviceCategoryId, int yearNumber);
        public List<MonthlyEnergyConsumptionLastYear> UserHistoryForYearByMonth(long userId, long deviceCategoryId, int yearNumber);
        public List<MonthlyEnergyConsumptionLastYear> DeviceHistoryForYearByMonth(long deviceId, int yearNumber);
        // PAGINACIJA
        public List<DailyEnergyConsumptionPastMonth> GetDailyEnergyUsageForPastMonthPagination(long deviceId, int pageNumber, int itemsPerPage);
        public List<DailyEnergyConsumptionPastMonth> GetProsumerDailyEnergyUsageForPastMonthPagination(long userId, long deviceCategoryId, int pageNumber, int itemsPerPage);
        public List<DailyEnergyConsumptionPastMonth> GetSettlementDailyEnergyUsageForPastMonthPagination(long settlementId, long deviceCategoryId, int pageNumber, int itemsPerPage);
        public List<DailyEnergyConsumptionPastMonth> GetCityDailyEnergyUsageForPastMonthPagination(long cityId, long deviceCategoryId, int pageNumber, int itemsPerPage);
        public List<EnergyToday> UserHistoryForThePastDayByHourPagination(long userId, long deviceCategoryId, int pageNumber, int itemsPerPage);
    }
}
