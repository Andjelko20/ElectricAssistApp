using Server.DTOs;

namespace Server.Services
{
    public interface IDSOService
    {
        public long GetCityId(string cityName);
        public List<SettlementDTO> GetSettlements(long cityId);
        public double GetCityConsumptionForToday(long cityId, long deviceCategoryId);
        public double GetUsageHistoryForDeviceInThisMonth(long cityId, long deviceCategoryId);
        public double GetUsageHistoryForDeviceInThisYear(long cityId, long deviceCategoryId);
        public List<EnergyToday> CalculateEnergyUsageForToday(long settlementId, long deviceCategoryId);
        public List<EnergyToday> CalculateEnergyUsageForTodayInCity(long cityId, long deviceCategoryId);
    }
}
