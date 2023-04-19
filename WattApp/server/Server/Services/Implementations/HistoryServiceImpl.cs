using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models;
using System.Threading;

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
            DateTime TheTime = DateTime.Now;

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= OneYearAgo && u.EndTime <= TheTime)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }

        public double GetUsageHistoryForDeviceInLastMonth(long deviceId)
        {
            DateTime OneMonthAgo = DateTime.Now.AddMonths(-1);
            DateTime TheTime = DateTime.Now;

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= OneMonthAgo && u.EndTime <= TheTime)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }

        public double GetUsageHistoryForDeviceInLastDay(long deviceId)
        {
            DateTime ADayAgo = DateTime.Now.AddDays(-1);
            DateTime TheTime = DateTime.Now;

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= ADayAgo && u.EndTime <= TheTime)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }

        public double GetUsageHistoryForDeviceInPastWeek(long deviceId)
        {
            DateTime AWeekAgo = DateTime.Now.AddDays(-7);
            DateTime TheTime = DateTime.Now;

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= AWeekAgo && u.EndTime <= TheTime)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }

        public double GetConsumptionForForwardedList(long deviceId, List<DeviceEnergyUsage> deviceEnergyUsageLista)
        {
            var Device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            var DeviceModel = _context.DeviceModels.FirstOrDefault(dm => dm.Id == Device.DeviceModelId);
            float EnergyInKwh = DeviceModel.EnergyKwh; // da li je null proverava se u kontroleru i vraca NotFound
            Console.WriteLine("''''''''''''''''''''''''' EnergyInKwh="+EnergyInKwh);
            double Consumption = 0.0;
            double Hours = -1;
            foreach (var item in deviceEnergyUsageLista)
            {
                Hours = Math.Abs((item.EndTime - item.StartTime).TotalHours);
                Console.WriteLine("'''''''''''''''''' Hours="+Hours);
                Consumption += (double)(EnergyInKwh * Hours);
                Console.WriteLine("''''''''''''''''''Consumption="+Consumption);
            }

            return Math.Round(Consumption, 2);
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
                 DateTime TheTime = DateTime.Now;

                 var UsageList = _context.DeviceEnergyUsages
                                 .Where(u => u.DeviceId == deviceId && u.StartTime >= StartDate && u.EndTime <= EndDate && u.EndTime <= TheTime)
                                 .OrderBy(u => u.StartTime)
                                 .ToList();

                double UsageInHours = 0.0;
                double UsageInKwh = 0.0;
                if (UsageList == null)
                {
                    Results.Insert(0, new MonthlyEnergyConsumptionLastYear
                    {
                        Month = StartDate.ToString("MMMM"),
                        Year = StartDate.Year,
                        EnergyUsageResult = UsageInKwh
                    });
                }
                else
                {
                    foreach (var item in UsageList)
                    {
                        var DeviceModel = _context.DeviceModels.FirstOrDefault(dm => dm.Id == Device.DeviceModelId);
                        float EnergyInKwh = DeviceModel.EnergyKwh;

                        UsageInHours = (item.EndTime - item.StartTime).TotalHours;
                        UsageInKwh += UsageInHours * EnergyInKwh; //Device.EnergyInKwh;
                    }
                    //Console.WriteLine("****** " + StartDate + " - " + UsageInKwh);
                    Results.Insert(0, new MonthlyEnergyConsumptionLastYear
                    {
                        Month = StartDate.ToString("MMMM"),
                        Year = StartDate.Year,
                        EnergyUsageResult = Math.Round(UsageInKwh, 2)
                    });
                }
            }
            return Results;
        }

        public List<DailyEnergyConsumptionPastMonth> GetDailyEnergyUsageForPastMonth(long deviceId)
        {
            var Device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            var DeviceModel = _context.DeviceModels.FirstOrDefault(dm => dm.Id == Device.DeviceModelId);
            float EnergyInKwh = DeviceModel.EnergyKwh;
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
                    EnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * EnergyInKwh;// Device.EnergyInKwh;

                Results.Add(new DailyEnergyConsumptionPastMonth
                {
                    Day = date.Day,
                    Month = date.ToString("MMMM"),
                    Year = date.Year,
                    EnergyUsageResult = Math.Round(EnergyUsage, 2)
                });
            }

            return Results;
        }

        public List<DailyEnergyConsumptionPastMonth> GetDailyEnergyUsageForPastWeek(long deviceId)
        {
            var Device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            var DeviceModel = _context.DeviceModels.FirstOrDefault(dm => dm.Id == Device.DeviceModelId);
            float EnergyInKwh = DeviceModel.EnergyKwh;
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
                    EnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * EnergyInKwh;// Device.EnergyInKwh;

                Results.Add(new DailyEnergyConsumptionPastMonth
                {
                    Day = date.Day,
                    Month = date.ToString("MMMM"),
                    Year = date.Year,
                    EnergyUsageResult = Math.Round(EnergyUsage, 2)
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
            //var devicesForUser = _context.Devices.Where(d => d.UserId == userId).ToList();
            var devicesForUser = _context.Devices
                                .Include(d => d.DeviceModel)
                                .ThenInclude(dm => dm.DeviceType)
                                .ThenInclude(dt => dt.DeviceCategory)
                                .Where(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId).ToList();
            
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

                var DeviceModel = _context.DeviceModels.FirstOrDefault(dm => dm.Id == device.DeviceModelId);
                float EnergyInKwh = DeviceModel.EnergyKwh;

                foreach (var usage in deviceUsageList)
                {
                    totalEnergyConsumption += (usage.EndTime - usage.StartTime).TotalHours * EnergyInKwh;// device.EnergyInKwh;
                }
            }

            return Math.Round(totalEnergyConsumption, 2);
        }

        // ZA PROSLEDJEN ID KORISNIKA POTROSNJA ZA GRAFIKE
        public List<MonthlyEnergyConsumptionLastYear> GetMonthlyEnergyUsageForPastYear(long userId, long deviceCategoryId)
        {
            //var userDevices = _context.Devices.Where(d => d.UserId == userId && d.DeviceCategoryId == deviceCategoryId).ToList();
            //var userDevices = _context.Devices.Where(d => d.UserId == userId).ToList();
            var userDevices = _context.Devices
                                .Include(d => d.DeviceModel)
                                .ThenInclude(dm => dm.DeviceType)
                                .ThenInclude(dt => dt.DeviceCategory)
                                .Where(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId).ToList();

            var endDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            var startDate = endDate.AddYears(-1);

            var monthlyUsage = new List<MonthlyEnergyConsumptionLastYear>();

            for (var i = 0; i < 12; i++)
            {
                var monthStartDate = startDate.AddMonths(i);
                //Console.WriteLine("***** monthStartDate: " + monthStartDate);
                var monthEndDate = monthStartDate.AddMonths(1).AddDays(-1).AddSeconds(1);
                var monthlyEnergyUsage = 0.0;

                foreach (var device in userDevices)
                {
                    var deviceUsages = _context.DeviceEnergyUsages
                        .Where(u => u.DeviceId == device.Id && u.StartTime >= monthStartDate && u.EndTime <= monthEndDate)
                        .ToList();

                    var DeviceModel = _context.DeviceModels.FirstOrDefault(dm => dm.Id == device.DeviceModelId);
                    float EnergyInKwh = DeviceModel.EnergyKwh;

                    foreach (var usage in deviceUsages)
                    {
                        monthlyEnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * EnergyInKwh;// device.EnergyInKwh;
                    }
                }

                monthlyUsage.Add(new MonthlyEnergyConsumptionLastYear
                {
                    Month = monthStartDate.ToString("MMMM"),
                    Year = monthStartDate.Year,
                    EnergyUsageResult = Math.Round(monthlyEnergyUsage, 2)
                });
            }

            return monthlyUsage;
        }

        public List<DailyEnergyConsumptionPastMonth> UserHistoryForThePastWeek(long userId, long deviceCategoryId)
        {
            //var userDevices = _context.Devices.Where(d => d.UserId == userId && d.DeviceCategoryId == deviceCategoryId).ToList();
            //var userDevices = _context.Devices.Where(d => d.UserId == userId).ToList();
            var userDevices = _context.Devices
                                .Include(d => d.DeviceModel)
                                .ThenInclude(dm => dm.DeviceType)
                                .ThenInclude(dt => dt.DeviceCategory)
                                .Where(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId).ToList();

            var EndDate = DateTime.Now.Date;//.AddDays(-1);
            var StartDate = EndDate.AddDays(-6);

            List<DeviceEnergyUsage> UsageList = new List<DeviceEnergyUsage>();
            var Results = new List<DailyEnergyConsumptionPastMonth>();

            foreach (var device in userDevices)
            {
                UsageList = _context.DeviceEnergyUsages
                            .Where(u => u.DeviceId == device.Id && u.StartTime >= StartDate && u.EndTime <= EndDate)
                            .ToList();

                var DeviceModel = _context.DeviceModels.FirstOrDefault(dm => dm.Id == device.DeviceModelId);
                float EnergyInKwh = DeviceModel.EnergyKwh;

                for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
                {
                    var UsageForDate = UsageList.Where(u => u.StartTime.Date == date.Date).ToList(); // za taj dan

                    double EnergyUsage = 0.0;
                    foreach (var usage in UsageForDate) // za taj dan
                        EnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * EnergyInKwh;// userDevice.EnergyInKwh; // za svaki period kada je radio izracunaj koliko je trosio

                    Results.Add(new DailyEnergyConsumptionPastMonth // klasa moze i za week
                    {
                        Day = date.Day,
                        Month = date.ToString("MMMM"),
                        Year = date.Year,
                        EnergyUsageResult = Math.Round(EnergyUsage, 2)
                    });
                }
            }

            var sumByDay = Results.GroupBy(r => new { r.Day, r.Month, r.Year })
                               .Select(g => new DailyEnergyConsumptionPastMonth
                               {
                                   Day = g.Key.Day,
                                   Month = g.Key.Month,
                                   Year = g.Key.Year,
                                   EnergyUsageResult = Math.Round(g.Sum(d => d.EnergyUsageResult), 2)
                               })
                               .ToList();

            return sumByDay;
        }

        public List<DailyEnergyConsumptionPastMonth> SettlementHistoryForThePastWeek(long settlementId, long deviceCategoryId)
        {
            var now = DateTime.Now;
            var settlementHistory = new List<DailyEnergyConsumptionPastMonth>();

            // pronalazimo kategoriju uredjaja
            var deviceCategory = _context.DeviceCategories
                                .FirstOrDefault(dc => dc.Id == deviceCategoryId);

            // pronalazimo sve tipove uredjaja koji pripadaju toj kategoriji
            var deviceTypeIds = _context.DeviceTypes
                .Where(dt => dt.CategoryId == deviceCategory.Id)
                .Select(dt => dt.Id)
                .ToList();

            // pronalazimo sve modele uredjaja koji pripadaju tim tipovima uredjaja
            var deviceModelIds = _context.DeviceModels
                .Where(dm => deviceTypeIds.Contains(dm.DeviceTypeId))
                .Select(dm => dm.Id)
                .ToList();

            // pronalazimo sve uredjaje koji koriste te modele uredjaja
            var devices = _context.Devices
                .Where(d => deviceModelIds.Contains(d.DeviceModelId))
                .Select(d => d.Id)
                .ToList();

            // pronalazimo naselje iz tabele Settlements
            var settlement = _context.Settlements
                .FirstOrDefault(s => s.Id == settlementId);

            for (int i = 0; i < 7; i++) //iteriramo kroz svaki dan u prethodnoj nedelji
            {
                var currentDay = now.AddDays(-i); //trenutni dan u iteraciji

                // pronalazimo uredjaje te kategorije u tabeli DeviceEnergyUsages i to ako su radili tog dana
                var usages = _context.DeviceEnergyUsages
                    .Where(u => devices.Contains(u.DeviceId) && u.StartTime <= currentDay.AddDays(1) && (u.EndTime >= currentDay || u.EndTime == null) && _context.Devices.Any(d => d.Id == u.DeviceId && _context.Users.Any(u => u.Id == d.UserId && u.SettlementId == settlement.Id)))
                    .ToList();

                var dailyTotalUsage = 0.0;
                foreach (var usage in usages)
                {
                    var usageStart = usage.StartTime;

                    var usageEnd = usage.EndTime;
                    if (usageEnd == null || usageEnd > currentDay.AddDays(1))
                    {
                        usageEnd = currentDay.AddDays(1);
                    }

                    var usageTime = (usageEnd - usageStart).TotalHours;
                    var deviceEnergyUsage = _context.Devices
                        .Include(d => d.DeviceModel)
                        .Where(d => d.Id == usage.DeviceId)
                        .Select(d => d.DeviceModel.EnergyKwh)
                        .FirstOrDefault();

                    dailyTotalUsage += deviceEnergyUsage * usageTime;
                }

                settlementHistory.Insert(0, new DailyEnergyConsumptionPastMonth
                {
                    Day = currentDay.Day,
                    Month = currentDay.ToString("MMMM"),
                    Year = currentDay.Year,
                    EnergyUsageResult = Math.Round(dailyTotalUsage, 2)
                });
            }

            return settlementHistory;
        }

        public List<DailyEnergyConsumptionPastMonth> CityHistoryForThePastWeek(long cityId, long deviceCategoryId)
        {
            var now = DateTime.Now;
            var cityHistory = new List<DailyEnergyConsumptionPastMonth>();

            // pronalazimo kategoriju uredjaja
            var deviceCategory = _context.DeviceCategories
                                .FirstOrDefault(dc => dc.Id == deviceCategoryId);

            // pronalazimo sve tipove uredjaja koji pripadaju toj kategoriji
            var deviceTypeIds = _context.DeviceTypes
                .Where(dt => dt.CategoryId == deviceCategory.Id)
                .Select(dt => dt.Id)
                .ToList();

            // pronalazimo sve modele uredjaja koji pripadaju tim tipovima uredjaja
            var deviceModelIds = _context.DeviceModels
                .Where(dm => deviceTypeIds.Contains(dm.DeviceTypeId))
                .Select(dm => dm.Id)
                .ToList();

            // pronalazimo sve uredjaje koji koriste te modele uredjaja i pripadaju datom naselju
            var devices = _context.Devices
                .Where(d => deviceModelIds.Contains(d.DeviceModelId) && _context.Users.Any(u => u.Id == d.UserId && u.Settlement.CityId == cityId))
                .Select(d => d.Id)
                .ToList();

            for (int i = 0; i < 7; i++) //iteriramo kroz svaki dan u prethodnoj nedelji
            {
                var currentDay = now.AddDays(-i); //trenutni dan u iteraciji

                // pronalazimo uredjaje te kategorije u tabeli DeviceEnergyUsages i to ako su radili tog dana
                var usages = _context.DeviceEnergyUsages
                    .Where(u => devices.Contains(u.DeviceId) && u.StartTime <= currentDay.AddDays(1) && (u.EndTime >= currentDay || u.EndTime == null))
                    .ToList();

                var dailyTotalUsage = 0.0;
                foreach (var usage in usages)
                {
                    var usageStart = usage.StartTime;

                    var usageEnd = usage.EndTime;
                    if (usageEnd == null || usageEnd > currentDay.AddDays(1))
                    {
                        usageEnd = currentDay.AddDays(1);
                    }

                    var usageTime = (usageEnd - usageStart).TotalHours;
                    var deviceEnergyUsage = _context.Devices
                        .Include(d => d.DeviceModel)
                        .Where(d => d.Id == usage.DeviceId)
                        .Select(d => d.DeviceModel.EnergyKwh)
                        .FirstOrDefault();

                    dailyTotalUsage += deviceEnergyUsage * usageTime;
                }

                cityHistory.Insert(0, new DailyEnergyConsumptionPastMonth
                {
                    Day = currentDay.Day,
                    Month = currentDay.ToString("MMMM"),
                    Year = currentDay.Year,
                    EnergyUsageResult = Math.Round(dailyTotalUsage, 2)
                });
            }

            return cityHistory;
        }

        public List<DailyEnergyConsumptionPastMonth> SettlementHistoryForThePastMonth(long settlementId, long deviceCategoryId)
        {
            var now = DateTime.Now;
            var settlementHistory = new List<DailyEnergyConsumptionPastMonth>();

            // pronalazimo kategoriju uredjaja
            var deviceCategory = _context.DeviceCategories
                                .FirstOrDefault(dc => dc.Id == deviceCategoryId);

            // pronalazimo sve tipove uredjaja koji pripadaju toj kategoriji
            var deviceTypeIds = _context.DeviceTypes
                .Where(dt => dt.CategoryId == deviceCategory.Id)
                .Select(dt => dt.Id)
                .ToList();

            // pronalazimo sve modele uredjaja koji pripadaju tim tipovima uredjaja
            var deviceModelIds = _context.DeviceModels
                .Where(dm => deviceTypeIds.Contains(dm.DeviceTypeId))
                .Select(dm => dm.Id)
                .ToList();

            // pronalazimo sve uredjaje koji koriste te modele uredjaja
            var devices = _context.Devices
                .Where(d => deviceModelIds.Contains(d.DeviceModelId))
                .Select(d => d.Id)
                .ToList();

            // pronalazimo naselje iz tabele Settlements
            var settlement = _context.Settlements
                .FirstOrDefault(s => s.Id == settlementId);

            for (int i = 0; i < 30; i++) //iteriramo kroz svaki dan u prethodnoj nedelji
            {
                var currentDay = now.AddDays(-i); //trenutni dan u iteraciji

                // pronalazimo uredjaje te kategorije u tabeli DeviceEnergyUsages i to ako su radili tog dana
                var usages = _context.DeviceEnergyUsages
                    .Where(u => devices.Contains(u.DeviceId) && u.StartTime <= currentDay.AddDays(1) && (u.EndTime >= currentDay || u.EndTime == null) && _context.Devices.Any(d => d.Id == u.DeviceId && _context.Users.Any(u => u.Id == d.UserId && u.SettlementId == settlement.Id)))
                    .ToList();

                var dailyTotalUsage = 0.0;
                foreach (var usage in usages)
                {
                    var usageStart = usage.StartTime;

                    var usageEnd = usage.EndTime;
                    if (usageEnd == null || usageEnd > currentDay.AddDays(1))
                    {
                        usageEnd = currentDay.AddDays(1);
                    }

                    var usageTime = (usageEnd - usageStart).TotalHours;
                    var deviceEnergyUsage = _context.Devices
                        .Include(d => d.DeviceModel)
                        .Where(d => d.Id == usage.DeviceId)
                        .Select(d => d.DeviceModel.EnergyKwh)
                        .FirstOrDefault();

                    dailyTotalUsage += deviceEnergyUsage * usageTime;
                }

                settlementHistory.Insert(0, new DailyEnergyConsumptionPastMonth
                {
                    Day = currentDay.Day,
                    Month = currentDay.ToString("MMMM"),
                    Year = currentDay.Year,
                    EnergyUsageResult = Math.Round(dailyTotalUsage, 2)
                });
            }

            return settlementHistory;
        }

        public List<DailyEnergyConsumptionPastMonth> CityHistoryForThePastMonth(long cityId, long deviceCategoryId)
        {
            var now = DateTime.Now;
            var cityHistory = new List<DailyEnergyConsumptionPastMonth>();

            // pronalazimo kategoriju uredjaja
            var deviceCategory = _context.DeviceCategories
                                .FirstOrDefault(dc => dc.Id == deviceCategoryId);

            // pronalazimo sve tipove uredjaja koji pripadaju toj kategoriji
            var deviceTypeIds = _context.DeviceTypes
                .Where(dt => dt.CategoryId == deviceCategory.Id)
                .Select(dt => dt.Id)
                .ToList();

            // pronalazimo sve modele uredjaja koji pripadaju tim tipovima uredjaja
            var deviceModelIds = _context.DeviceModels
                .Where(dm => deviceTypeIds.Contains(dm.DeviceTypeId))
                .Select(dm => dm.Id)
                .ToList();

            // pronalazimo sve uredjaje koji koriste te modele uredjaja i pripadaju datom naselju
            var devices = _context.Devices
                .Where(d => deviceModelIds.Contains(d.DeviceModelId) && _context.Users.Any(u => u.Id == d.UserId && u.Settlement.CityId == cityId))
                .Select(d => d.Id)
                .ToList();

            for (int i = 0; i < 30; i++) //iteriramo kroz svaki dan u prethodnoj nedelji
            {
                var currentDay = now.AddDays(-i); //trenutni dan u iteraciji

                // pronalazimo uredjaje te kategorije u tabeli DeviceEnergyUsages i to ako su radili tog dana
                var usages = _context.DeviceEnergyUsages
                    .Where(u => devices.Contains(u.DeviceId) && u.StartTime <= currentDay.AddDays(1) && (u.EndTime >= currentDay || u.EndTime == null))
                    .ToList();

                var dailyTotalUsage = 0.0;
                foreach (var usage in usages)
                {
                    var usageStart = usage.StartTime;

                    var usageEnd = usage.EndTime;
                    if (usageEnd == null || usageEnd > currentDay.AddDays(1))
                    {
                        usageEnd = currentDay.AddDays(1);
                    }

                    var usageTime = (usageEnd - usageStart).TotalHours;
                    var deviceEnergyUsage = _context.Devices
                        .Include(d => d.DeviceModel)
                        .Where(d => d.Id == usage.DeviceId)
                        .Select(d => d.DeviceModel.EnergyKwh)
                        .FirstOrDefault();

                    dailyTotalUsage += deviceEnergyUsage * usageTime;
                }

                cityHistory.Insert(0, new DailyEnergyConsumptionPastMonth
                {
                    Day = currentDay.Day,
                    Month = currentDay.ToString("MMMM"),
                    Year = currentDay.Year,
                    EnergyUsageResult = Math.Round(dailyTotalUsage, 2)
                });
            }

            return cityHistory;
        }

        public List<MonthlyEnergyConsumptionLastYear> CityHistoryForThePastYearByMonth(long cityId, long deviceCategoryId)
        {
            var now = DateTime.Now;
            var monthlyEnergyConsumption = new List<MonthlyEnergyConsumptionLastYear>();

            // Pronalazimo kategoriju uređaja
            var deviceCategory = _context.DeviceCategories.FirstOrDefault(dc => dc.Id == deviceCategoryId);

            // Pronalazimo sve tipove uređaja koji pripadaju toj kategoriji
            var deviceTypeIds = _context.DeviceTypes
                .Where(dt => dt.CategoryId == deviceCategory.Id)
                .Select(dt => dt.Id)
                .ToList();

            // Pronalazimo sve modele uređaja koji pripadaju tim tipovima uređaja
            var deviceModelIds = _context.DeviceModels
                .Where(dm => deviceTypeIds.Contains(dm.DeviceTypeId))
                .Select(dm => dm.Id)
                .ToList();

            // Pronalazimo sve uređaje koji koriste te modele uređaja
            var devices = _context.Devices
                .Where(d => deviceModelIds.Contains(d.DeviceModelId))
                .Select(d => d.Id)
                .ToList();

            // Pronalazimo grad iz tabele Cities
            var city = _context.Cities.FirstOrDefault(c => c.Id == cityId);

            for (int i = 0; i < 12; i++) // Iteriramo kroz svaki mesec u prethodnoj godini
            {
                var currentMonth = now.AddMonths(-i); // Trenutni mesec u iteraciji

                // Pronalazimo uređaje te kategorije u tabeli DeviceEnergyUsages i to ako su radili tog meseca
                var usages = _context.DeviceEnergyUsages
                    .Where(u => devices.Contains(u.DeviceId) && u.StartTime <= currentMonth.AddMonths(1) && (u.EndTime >= currentMonth || u.EndTime == null) && _context.Devices.Any(d => d.Id == u.DeviceId && _context.Users.Any(u => u.Id == d.UserId && u.Settlement.CityId == city.Id)))
                    .ToList();

                var monthlyTotalUsage = 0.0;
                foreach (var usage in usages)
                {
                    var usageStart = usage.StartTime;

                    var usageEnd = usage.EndTime;
                    if (usageEnd == null || usageEnd > currentMonth.AddMonths(1))
                    {
                        usageEnd = currentMonth.AddMonths(1);
                    }

                    var usageTime = (usageEnd - usageStart).TotalHours;
                    var deviceEnergyUsage = _context.Devices
                        .Include(d => d.DeviceModel)
                        .Where(d => d.Id == usage.DeviceId)
                        .Select(d => d.DeviceModel.EnergyKwh)
                        .FirstOrDefault();

                    monthlyTotalUsage += deviceEnergyUsage * usageTime;
                }

                monthlyEnergyConsumption.Insert(0, new MonthlyEnergyConsumptionLastYear
                {
                    Month = currentMonth.ToString("MMMM"),
                    Year = currentMonth.Year,
                    EnergyUsageResult = Math.Round(monthlyTotalUsage, 2)
                });
            }

            return monthlyEnergyConsumption;
        }

        public List<MonthlyEnergyConsumptionLastYear> SettlementHistoryForThePastYearByMonth(long settlementId, long deviceCategoryId)
        {
            var now = DateTime.Now;
            var monthlyEnergyConsumption = new List<MonthlyEnergyConsumptionLastYear>();

            // pronalazimo kategoriju uredjaja
            var deviceCategory = _context.DeviceCategories
                .FirstOrDefault(dc => dc.Id == deviceCategoryId);

            // pronalazimo sve tipove uredjaja koji pripadaju toj kategoriji
            var deviceTypeIds = _context.DeviceTypes
                .Where(dt => dt.CategoryId == deviceCategory.Id)
                .Select(dt => dt.Id)
                .ToList();

            // pronalazimo sve modele uredjaja koji pripadaju tim tipovima uredjaja
            var deviceModelIds = _context.DeviceModels
                .Where(dm => deviceTypeIds.Contains(dm.DeviceTypeId))
                .Select(dm => dm.Id)
                .ToList();

            // pronalazimo sve uredjaje koji koriste te modele uredjaja
            var devices = _context.Devices
                .Where(d => deviceModelIds.Contains(d.DeviceModelId))
                .Select(d => d.Id)
                .ToList();

            // pronalazimo naselje iz tabele Settlements
            var settlement = _context.Settlements
                .FirstOrDefault(s => s.Id == settlementId);

            for (int i = 0; i < 12; i++) // iteriramo kroz svaki mesec u prethodnoj godini
            {
                var currentMonth = now.AddMonths(-i); // trenutni mesec u iteraciji

                // pronalazimo uredjaje te kategorije u tabeli DeviceEnergyUsages i to ako su radili u tom mesecu
                var usages = _context.DeviceEnergyUsages
                    .Where(u => devices.Contains(u.DeviceId) && u.StartTime <= currentMonth.AddMonths(1) && (u.EndTime >= currentMonth || u.EndTime == null) && _context.Devices.Any(d => d.Id == u.DeviceId && _context.Users.Any(u => u.Id == d.UserId && u.SettlementId == settlement.Id)))
                    .ToList();

                var monthlyTotalUsage = 0.0;
                foreach (var usage in usages)
                {
                    var usageStart = usage.StartTime;

                    var usageEnd = usage.EndTime;
                    if (usageEnd == null || usageEnd > currentMonth.AddMonths(1))
                    {
                        usageEnd = currentMonth.AddMonths(1);
                    }

                    var usageTime = (usageEnd - usageStart).TotalHours;
                    var deviceEnergyUsage = _context.Devices
                        .Include(d => d.DeviceModel)
                        .Where(d => d.Id == usage.DeviceId)
                        .Select(d => d.DeviceModel.EnergyKwh)
                        .FirstOrDefault();

                    monthlyTotalUsage += deviceEnergyUsage * usageTime;
                }

                monthlyEnergyConsumption.Insert(0, new MonthlyEnergyConsumptionLastYear
                {
                    Month = currentMonth.ToString("MMMM"),
                    Year = currentMonth.Year,
                    EnergyUsageResult = Math.Round(monthlyTotalUsage, 2)
                });
            }

            return monthlyEnergyConsumption;
        }

        public double GetUsageHistoryForDeviceInThisMonth(long deviceId)
        {
            DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            DateTime endTime = DateTime.Now;

            List<DeviceEnergyUsage> deviceEnergyUsageList = _context.DeviceEnergyUsages
                .Where(u => u.DeviceId == deviceId && u.StartTime >= startOfMonth/* && u.EndTime <= endTime*/)
                .ToList();

            foreach (var usage in deviceEnergyUsageList)
            {
                if (usage.EndTime > endTime)
                    usage.EndTime = endTime;

                Console.WriteLine("-------------++++++++++++++++++---------- deviceId="+usage.DeviceId+" --- startTime="+usage.StartTime+" --- endTime="+usage.EndTime);
            }

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageList);
        }

        public double GetUsageHistoryForDeviceToday(long deviceId)
        {
            // za trazeni uredjaj, samo kada je radio tokom danasenjeg dana
            var deviceEnergyUsages = _context.DeviceEnergyUsages
                .Where(usage => usage.DeviceId == deviceId && usage.StartTime.Date == DateTime.Today)
                .ToList();

            foreach (var usage in deviceEnergyUsages)
            {
                // od 00:00h do ovog trenutka, danasnjeg dana
                if (usage.EndTime > DateTime.Now)
                    usage.EndTime = DateTime.Now;

                Console.WriteLine("-------------++++++++++++++++++---------- deviceId=" + usage.DeviceId + " --- startTime=" + usage.StartTime + " --- endTime=" + usage.EndTime);
            }

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsages);
        }

        public double GetUsageHistoryForDeviceThisYear(long deviceId)
        {
            DateTime startOfTheYear = new DateTime(DateTime.Now.Year, 1, 1);
            Console.WriteLine("----------------------- startOfTheYear="+startOfTheYear);
            // za trazeni uredjaj, samo kada je radio od pocetka ove godine
            var deviceEnergyUsages = _context.DeviceEnergyUsages
                .Where(usage => usage.DeviceId == deviceId && usage.StartTime.Date >= startOfTheYear)
                .ToList();

            foreach (var usage in deviceEnergyUsages)
            {
                // od 01.01.2023.(trenutne godine) 00:00:00h do ovog trenutka, danasnjeg dana
                if (usage.EndTime > DateTime.Now)
                    usage.EndTime = DateTime.Now;

                Console.WriteLine("-------------++++++++++++++++++---------- deviceId=" + usage.DeviceId + " --- startTime=" + usage.StartTime + " --- endTime=" + usage.EndTime);
            }

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsages);
        }

        public double GetUsageHistoryForDeviceForPreviousMonth(long deviceId)
        {
            DateTime startOfThePreviousMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month-1, 1);
            DateTime startOfTheCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endOfThePreviousMonth = startOfTheCurrentMonth.AddSeconds(-1);
            Console.WriteLine("----------------------- startOfThePreviousMonth=" + startOfThePreviousMonth);
            Console.WriteLine("----------------------- startOfThePreviousMonth=" + endOfThePreviousMonth);
            // za trazeni uredjaj, samo kada je radio od pocetka do kraja prethodnog meseca
            var deviceEnergyUsages = _context.DeviceEnergyUsages
                .Where(usage => usage.DeviceId == deviceId && usage.StartTime.Date >= startOfThePreviousMonth && usage.EndTime <= endOfThePreviousMonth)
                .ToList();

            foreach (var usage in deviceEnergyUsages)
            {
                // od 01. u prethodnom mesecu od 00:00:00h do 23:59:59h poslednjeg dana u mesecu
                /*if (usage.EndTime > endOfThePreviousMonth)
                    usage.EndTime = endOfThePreviousMonth;*/

                Console.WriteLine("-------------++++++++++++++++++---------- deviceId=" + usage.DeviceId + " --- startTime=" + usage.StartTime + " --- endTime=" + usage.EndTime);
            }

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsages);
        }
    }
}
