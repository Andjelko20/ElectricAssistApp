using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models;

namespace Server.Services.Implementations
{
    public class HistoryServiceImpl : IHistoryService
    {
        private readonly SqliteDbContext _context;
        public HistoryServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public double GetUsageHistoryForDeviceInLastYear(int deviceId)
        {
            DateTime OneYearAgo = DateTime.Now.AddYears(-1); // tip DATETIME, trenutna godina 2023. -1 = 2022.

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= OneYearAgo)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }

        public double GetUsageHistoryForDeviceInLastMonth(int deviceId)
        {
            DateTime OneMonthAgo = DateTime.Now.AddMonths(-1);

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= OneMonthAgo)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }

        public double GetUsageHistoryForDeviceInLastDay(int deviceId)
        {
            DateTime ADayAgo = DateTime.Now.AddDays(-1);

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= ADayAgo)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }

        public double GetUsageHistoryForDeviceInPastWeek(int deviceId)
        {
            DateTime AWeekAgo = DateTime.Now.AddDays(-7);

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= AWeekAgo)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }

        public double GetConsumptionForForwardedList(int deviceId, List<DeviceEnergyUsage> deviceEnergyUsageLista)
        {
            var Device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            float EnergyInKwh = Device.EnergyInKwh; // da li je null proverava se u kontroleru i vraca NotFound

            double Consumption = 0.0;
            double Hours = -1;
            foreach (var item in deviceEnergyUsageLista)
            {
                Hours = (item.EndTime - item.StartTime).TotalHours;
                Consumption += (double)Math.Round(EnergyInKwh * Hours, 2);
            }

            return Consumption;
        }

        // za bar plot, istorija za godinu dana, prikaz po mesecima
        public List<MonthlyEnergyConsumptionLastYear> GetMonthlyEnergyUsage(int deviceId)
        {
            var Device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();

            var Results = new List<MonthlyEnergyConsumptionLastYear>();
            for (int i = 0; i < 12; i++)
            {
                var StartDate = DateTime.Now.AddMonths(-i).Date;
                var EndDate = StartDate.AddMonths(1).AddDays(-1).Date.AddDays(1).AddSeconds(-1);

                var UsageList = _context.DeviceEnergyUsages
                                .Where(u => u.DeviceId == deviceId && u.StartTime >= StartDate && u.EndTime <= EndDate)
                                .OrderBy(u => u.StartTime)
                                .ToList();

                double UsageInHours = 0.0;
                double UsageInKwh = 0.0;
                if (UsageList == null)
                {
                    Results.Add(new MonthlyEnergyConsumptionLastYear
                    {
                        Month = StartDate.ToString("MMMM yyyy"),
                        EnergyUsageResult = UsageInKwh
                    });
                }
                else
                {
                    foreach (var item in UsageList)
                    {
                        UsageInHours = (item.EndTime - item.StartTime).TotalHours;
                        UsageInKwh += UsageInHours * Device.EnergyInKwh;
                    }

                    Results.Insert(0, new MonthlyEnergyConsumptionLastYear
                    {
                        Month = StartDate.ToString("MMMM yyyy"),
                        EnergyUsageResult = UsageInKwh
                    });
                } 
            }
            return Results;
        }

        public List<DailyEnergyConsumptionPastMonth> GetDailyEnergyUsageForPastMonth(int deviceId)
        {
            var Device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            var EndDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            var StartDate = EndDate.AddDays(-30);

            var UsageList = _context.DeviceEnergyUsages
                            .Where(u => u.DeviceId == deviceId && u.StartTime >= StartDate && u.EndTime <= EndDate)
                            .ToList();

            var Results = new List<DailyEnergyConsumptionPastMonth>();

            for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
            {
                var UsageForDate = UsageList.Where(u => u.StartTime.Date == date.Date ).ToList();
                
                double EnergyUsage = 0.0;
                foreach (var usage in UsageForDate)
                    EnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * Device.EnergyInKwh;

                Results.Add(new DailyEnergyConsumptionPastMonth
                {
                    Day = date.ToString("dd.MM.yyyy"),
                    EnergyUsageResult = EnergyUsage
                });
            }

            return Results;
        }

        public List<DailyEnergyConsumptionPastMonth> GetDailyEnergyUsageForPastWeek(int deviceId)
        {
            var Device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            var EndDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            var StartDate = EndDate.AddDays(-6);

            var UsageList = _context.DeviceEnergyUsages
                            .Where(u => u.DeviceId == deviceId && u.StartTime >= StartDate && u.EndTime <= EndDate)
                            .ToList();

            var Results = new List<DailyEnergyConsumptionPastMonth>();

            for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
            {
                var UsageForDate = UsageList.Where(u => u.StartTime.Date == date.Date).ToList();

                double EnergyUsage = 0.0;
                foreach (var usage in UsageForDate)
                    EnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * Device.EnergyInKwh;

                Results.Add(new DailyEnergyConsumptionPastMonth
                {
                    Day = date.ToString("dd.MM.yyyy"),
                    EnergyUsageResult = EnergyUsage
                });
            }

            return Results;
        }
    }
}
