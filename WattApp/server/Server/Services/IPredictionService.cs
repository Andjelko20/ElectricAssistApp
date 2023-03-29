using Server.DTOs;

namespace Server.Services
{
    public interface IPredictionService
    {
        public List<DailyEnergyConsumptionPastMonth> ConsumptionPredictionForTheNextWeek(int deviceId);
    }
}
