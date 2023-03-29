using Server.DTOs;

namespace Server.Services
{
    public interface IPredictionService
    {
        public List<DailyEnergyConsumptionPastMonth> ConsumptionPredictionForTheNextWeek(int deviceId);
        public List<DailyEnergyConsumptionPastMonth> UserPredictionForTheNextWeek(int userId);
    }
}
