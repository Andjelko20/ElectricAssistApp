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

        public double GetUsageHistoryForDeviceInLastYear(long deviceId)
        {
            DateTime OneYearAgo = DateTime.Now.AddYears(-1); // tip DATETIME, trenutna godina 2023. -1 = 2022.

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= OneYearAgo)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }

        public double GetUsageHistoryForDeviceInLastMonth(long deviceId)
        {
            DateTime OneMonthAgo = DateTime.Now.AddMonths(-1);

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= OneMonthAgo)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }

        public double GetUsageHistoryForDeviceInLastDay(long deviceId)
        {
            DateTime ADayAgo = DateTime.Now.AddDays(-1);

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= ADayAgo)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }

        public double GetUsageHistoryForDeviceInPastWeek(long deviceId)
        {
            DateTime AWeekAgo = DateTime.Now.AddDays(-7);

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= AWeekAgo)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }

        public double GetConsumptionForForwardedList(long deviceId, List<DeviceEnergyUsage> deviceEnergyUsageLista)
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
        public List<MonthlyEnergyConsumptionLastYear> GetMonthlyEnergyUsage(long deviceId)
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

        public List<DailyEnergyConsumptionPastMonth> GetDailyEnergyUsageForPastMonth(long deviceId)
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

        public List<DailyEnergyConsumptionPastMonth> GetDailyEnergyUsageForPastWeek(long deviceId)
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


        // ZA PROSLEDJEN ID KORISNIKA
        public double GetTotalEnergyConsumptionForUser(long userId, long deviceCategoryId)
        {
            long daysInPast = 0;
            return SumEnergyConsumption(userId, daysInPast, deviceCategoryId);
        }

        public double GetUserEnergyConsumptionForPastDay(long userId, long deviceCategoryId)
        {
            long daysInPast = -1;
            return SumEnergyConsumption(userId, daysInPast, deviceCategoryId);
        }

        public double GetUserEnergyConsumptionForPastWeek(long userId, long deviceCategoryId)
        {
            long daysInPast = -6;
            return SumEnergyConsumption(userId, daysInPast, deviceCategoryId);
        }

        public double GetUserEnergyConsumptionForPastMonth(long userId, long deviceCategoryId)
        {
            long daysInPast = -30;
            return SumEnergyConsumption(userId, daysInPast, deviceCategoryId);
        }

        public double GetUserEnergyConsumptionForPastYear(long userId, long deviceCategoryId)
        {
            long daysInPast = -365;
            return SumEnergyConsumption(userId, daysInPast, deviceCategoryId);
        }

        public double SumEnergyConsumption(long userId, long daysInPast, long deviceCategoryId)
        {
            //var devicesForUser = _context.Devices.Where(d => d.UserId == userId && d.DeviceCategoryId == deviceCategoryId).ToList();
			var devicesForUser = _context.Devices.Where(d => d.UserId == userId).ToList();
            if (devicesForUser.Count == 0)
            {
                return 0;
            }

            var deviceIds = devicesForUser.Select(d => d.Id).ToList();
            var usageList = new List<DeviceEnergyUsage>();
            if (daysInPast != 0)
            {
                var currentDate = DateTime.Now.Date;
                var EndDate = currentDate.AddDays(1).AddSeconds(-1);
                var StartDate = currentDate.AddDays(daysInPast);

                usageList = _context.DeviceEnergyUsages
                            .Where(u => deviceIds.Contains(u.DeviceId) && u.StartTime >= StartDate && u.EndTime <= EndDate)
                            .ToList();
            }
            else // znaci da nam treba totalna potrosnja
            {
                usageList = _context.DeviceEnergyUsages
                            .Where(u => deviceIds.Contains(u.DeviceId))
                            .ToList();
            }

            var totalEnergyConsumption = 0.0;

            foreach (var device in devicesForUser)
            {
                var deviceUsageList = usageList.Where(u => u.DeviceId == device.Id).ToList();

                foreach (var usage in deviceUsageList)
                {
                    totalEnergyConsumption += (usage.EndTime - usage.StartTime).TotalHours * device.EnergyInKwh;
                }
            }

            return totalEnergyConsumption;
        }

        // ZA PROSLEDJEN ID KORISNIKA POTROSNJA ZA GRAFIKE
        public List<MonthlyEnergyConsumptionLastYear> GetMonthlyEnergyUsageForPastYear(long userId, long deviceCategoryId)
        {
            //var userDevices = _context.Devices.Where(d => d.UserId == userId && d.DeviceCategoryId == deviceCategoryId).ToList();
			var userDevices = _context.Devices.Where(d => d.UserId == userId).ToList();
            var endDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            var startDate = endDate.AddYears(-1);

            var monthlyUsage = new List<MonthlyEnergyConsumptionLastYear>();

            for (var i = 0; i < 12; i++)
            {
                var monthStartDate = startDate.AddMonths(i);
                Console.WriteLine("***** monthStartDate: " + monthStartDate);
                var monthEndDate = monthStartDate.AddMonths(1).AddDays(-1).AddSeconds(1);
                var monthlyEnergyUsage = 0.0;

                foreach (var device in userDevices)
                {
                    var deviceUsages = _context.DeviceEnergyUsages
                        .Where(u => u.DeviceId == device.Id && u.StartTime >= monthStartDate && u.EndTime <= monthEndDate)
                        .ToList();

                    foreach (var usage in deviceUsages)
                    {
                        monthlyEnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * device.EnergyInKwh;
                    }
                }

                monthlyUsage.Add(new MonthlyEnergyConsumptionLastYear
                {
                    Month = monthStartDate.ToString("MMMM yyyy"),
                    EnergyUsageResult = monthlyEnergyUsage
                });
            }

            return monthlyUsage;
        }

        public List<DailyEnergyConsumptionPastMonth> UserHistoryForThePastWeek(long userId, long deviceCategoryId)
        {
            //var userDevices = _context.Devices.Where(d => d.UserId == userId && d.DeviceCategoryId == deviceCategoryId).ToList();
			var userDevices = _context.Devices.Where(d => d.UserId == userId).ToList();
            var EndDate = DateTime.Now.Date.AddDays(-1);
            var StartDate = EndDate.AddDays(-6);

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
