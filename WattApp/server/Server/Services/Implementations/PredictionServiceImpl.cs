using Server.Data;
using Server.DTOs;

namespace Server.Services.Implementations
{
    public class PredictionServiceImpl : IPredictionService
    {
        private readonly SqliteDbContext _context;
        public PredictionServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public List<DailyEnergyConsumptionPastMonth> ConsumptionPredictionForTheNextWeek(int deviceId)
        {
            var Device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            var StartDate = DateTime.Now.Date.AddDays(1);
            var EndDate = StartDate.AddDays(7).AddSeconds(-1);

            var UsageList = _context.DeviceEnergyUsages
                            .Where(u => u.DeviceId == deviceId && u.StartTime >= StartDate && u.EndTime <= EndDate)
                            .ToList();

            var Results = new List<DailyEnergyConsumptionPastMonth>();

            for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
            {
                var UsageForDate = UsageList.Where(u => u.StartTime.Date == date.Date).ToList(); // za taj dan

                double EnergyUsage = 0.0;
                foreach (var usage in UsageForDate) // za taj dan
                    EnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * Device.EnergyInKwh; // za svaki period kada je radio izracunaj koliko je trosio

                Results.Add(new DailyEnergyConsumptionPastMonth // klasa moze i za week
                {
                    Day = date.ToString("dd.MM.yyyy"),
                    EnergyUsageResult = EnergyUsage
                });
            }

            return Results;
        }
    }
}
