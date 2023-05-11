using Microsoft.AspNetCore.Mvc;
using Server.DTOs;

namespace Server.Services
{
    public interface IHistoryFromToService
    {
        public double GetCityDoubleHistoryFromTo(string fromDate, string toDate, long deviceCategoryId, long cityId);
        public double GetSettlementDoubleHistoryFromTo(string fromDate, string toDate, long deviceCategoryId, long settlementId);
        public List<DailyEnergyConsumptionPastMonth> GetCityHistoryByDayFromTo(string fromDate, string toDate, long deviceCategoryId, long cityId);
        public List<MonthlyEnergyConsumptionLastYear> GetSettlementHistoryByMonthFromTo(string fromDate, string toDate, long deviceCategoryId, long settlementId);
        public List<DailyEnergyConsumptionPastMonth> GetSettlementHistoryByDayFromTo(string fromDate, string toDate, long deviceCategoryId, long settlementId);
        public List<EnergyToday> GetSettlementHistoryByHourFromTo(string fromDate, string toDate, long deviceCategoryId, long settlementId);
        public List<MonthlyEnergyConsumptionLastYear> GetCityHistoryByMonthFromTo(string fromDate, string toDate, long deviceCategoryId, long cityId);
        public List<EnergyToday> GetCityHistoryByHourFromTo(string fromDate, string toDate, long deviceCategoryId, long cityId);
        public List<MonthlyEnergyConsumptionLastYear> GetProsumerHistoryByMonthFromTo(string fromDate, string toDate, long userId, long categoryId);
        public List<EnergyToday> GetProsumerHistoryByHourFromTo(string fromDate, string toDate, long userId, long deviceCategoryId);
        public List<DailyEnergyConsumptionPastMonth> GetProsumerHistoryByDayFromTo(string fromDate, string toDate, long userId, long deviceCategoryId);
        public double GetProsumerDoubleHistoryFromTo(string fromDate, string toDate, long userId, long deviceCategoryId);
        public double GetDeviceDoubleHistoryFromTo(string fromDate, string toDate, long deviceId);
        public List<MonthlyEnergyConsumptionLastYear> GetDeviceHistoryByMonthFromTo(string fromDate, string toDate, long deviceId);
        public List<DailyEnergyConsumptionPastMonth> GetDeviceHistoryByDayFromTo(string fromDate, string toDate, long deviceId);
        public List<EnergyToday> GetDeviceHistoryByHourFromTo(string fromDate, string toDate, long deviceId);

        // PAGINACIJA
        public List<EnergyToday> GetDeviceHistoryByHourFromToPagination(string fromDate, string toDate, long deviceId, int pageNumber, int itemsPerPage);
        public List<EnergyToday> GetUserHistoryByHourFromToPagination(string fromDate, string toDate, long userId, long deviceCategoryId, int pageNumber, int itemsPerPage);
        public List<EnergyToday> GetSettlementHistoryByHourFromToPagination(string fromDate, string toDate, long settlementId, long deviceCategoryId, int pageNumber, int itemsPerPage);
        public List<EnergyToday> GetCityHistoryByHourFromToPagination(string fromDate, string toDate, long cityId, long deviceCategoryId, int pageNumber, int itemsPerPage);
        public List<DailyEnergyConsumptionPastMonth> GetUserHistoryByDayFromToPagination(string fromDate, string toDate, long userId, long deviceCategoryId, int pageNumber, int itemsPerPage);
        public List<DailyEnergyConsumptionPastMonth> GetDeviceHistoryByDayFromToPagination(string fromDate, string toDate, long deviceId, int pageNumber, int itemsPerPage);
        public List<DailyEnergyConsumptionPastMonth> GetSettlementHistoryByDayFromToPagination(string fromDate, string toDate, long settlementId, long deviceCategoryId, int pageNumber, int itemsPerPage);
    }
}
