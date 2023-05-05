using Server.DTOs;

namespace Server.Services
{
    public interface IPredictionService
    {
        public List<DailyEnergyConsumptionPastMonth> ConsumptionPredictionForTheNextWeek(long deviceId);
        public List<DailyEnergyConsumptionPastMonth> UserPredictionForTheNextWeek(long userId, long deviceCategoryId);
        public List<DailyEnergyConsumptionPastMonth> CityPredictionForTheNextWeek(long cityId, long deviceCategoryId);
        public List<DailyEnergyConsumptionPastMonth> SettlementPredictionForTheNextWeek(long settlementId, long deviceCategoryId);
        public List<DailyEnergyConsumptionPastMonth> CityPredictionForThePastWeek(long cityId, long deviceCategoryId);
        public List<DailyEnergyConsumptionPastMonth> SettlementPredictionForThePastWeek(long settlementId, long deviceCategoryId);



    }
}
