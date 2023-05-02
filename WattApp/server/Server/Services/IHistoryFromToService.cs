using Microsoft.AspNetCore.Mvc;
using Server.DTOs;

namespace Server.Services
{
    public interface IHistoryFromToService
    {
        public double GetCityDoubleHistoryFromTo(string fromDate, string toDate, long deviceCategoryId, long cityId);
        public double GetSettlementDoubleHistoryFromTo(string fromDate, string toDate, long deviceCategoryId, long settlementId);
        public List<DailyEnergyConsumptionPastMonth> GetCityHistoryByDayFromTo(string fromDate, string toDate, long deviceCategoryId, long cityId);
        public List<DailyEnergyConsumptionPastMonth> GetSettlementHistoryByDayFromTo(string fromDate, string toDate, long deviceCategoryId, long settlementId);
        public List<EnergyToday> GetSettlementHistoryByHourFromTo(string fromDate, string toDate, long deviceCategoryId, long settlementId);
        public List<EnergyToday> GetCityHistoryByHourFromTo(string fromDate, string toDate, long deviceCategoryId, long cityId);
        public List<EnergyToday> GetProsumerHistoryByHourFromTo(string fromDate, string toDate, long userId);





    }
}
