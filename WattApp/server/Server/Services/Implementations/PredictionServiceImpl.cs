using Server.Data;
using Server.DTOs;
using Server.Models;

namespace Server.Services.Implementations
{
    public class PredictionServiceImpl : IPredictionService
    {
        private readonly SqliteDbContext _context;
        public PredictionServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public List<DailyEnergyConsumptionPastMonth> ConsumptionPredictionForTheNextWeek(long deviceId)
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

        public List<DailyEnergyConsumptionPastMonth> UserPredictionForTheNextWeek(long userId, long deviceCategoryId)
        {
            //var userDevices = _context.Devices.Where(d => d.UserId == userId && d.DeviceCategoryId == deviceCategoryId).ToList();
			var userDevices = _context.Devices.Where(d => d.UserId == userId).ToList();
            var StartDate = DateTime.Now.Date.AddDays(1);
            var EndDate = StartDate.AddDays(7).AddSeconds(-1);


            List<DeviceEnergyUsage> UsageList = new List<DeviceEnergyUsage>();
            var Results = new List<DailyEnergyConsumptionPastMonth>();

            foreach (var userDevice in userDevices)
            {
                UsageList = _context.DeviceEnergyUsages
                            .Where(u => u.DeviceId == userDevice.Id && u.StartTime >= StartDate && u.EndTime <= EndDate)
                            .ToList();

                for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
                {
                    var UsageForDate = UsageList.Where(u => u.StartTime.Date == date.Date).ToList(); // za taj dan

                    double EnergyUsage = 0.0;
                    foreach (var usage in UsageForDate) // za taj dan
                        EnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * userDevice.EnergyInKwh; // za svaki period kada je radio izracunaj koliko je trosio

                    Results.Add(new DailyEnergyConsumptionPastMonth // klasa moze i za week
                    {
                        Day = date.ToString("dd.MM.yyyy"),
                        EnergyUsageResult = EnergyUsage
                    });
                }
            }

            //return Results;

            var sumByDay = Results.GroupBy(r => r.Day)
                                .Select(g => new DailyEnergyConsumptionPastMonth
                                {
                                    Day = g.Key,
                                    EnergyUsageResult = g.Sum(d => d.EnergyUsageResult)
                                })
                                .ToList();

            return sumByDay;
        }
    }
}
