using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;
using System;
using System.Globalization;
using System.Linq;
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
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);
            DateTime theTime = DateTime.Now;
            double consumption = 0.0;

            var deviceModelEnergyKwh = _context.Devices
                .Where(d => d.Id == deviceId)
                .Select(d => d.DeviceModel.EnergyKwh)
                .FirstOrDefault();

            var energyUsages = _context.DeviceEnergyUsages
                .Where(du => du.DeviceId == deviceId && du.StartTime >= oneYearAgo && du.EndTime <= theTime)
                .ToList();

            foreach (var energyUsage in energyUsages)
            {
                var hours = Math.Abs((energyUsage.EndTime - energyUsage.StartTime).TotalHours);
                consumption += (double)(deviceModelEnergyKwh * hours);
            }

            return Math.Round(consumption, 2);
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
            //var Device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            var Device = _context.Devices.Find(deviceId);
            var DeviceModel = _context.DeviceModels.Find(Device.DeviceModelId);
            float EnergyInKwh = DeviceModel.EnergyKwh;

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
                        //var DeviceModel = _context.DeviceModels.FirstOrDefault(dm => dm.Id == Device.DeviceModelId);
                        //float EnergyInKwh = DeviceModel.EnergyKwh;

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

        public List<DailyEnergyConsumptionPastMonth> UserHistoryForThePastMonth(long userId, long deviceCategoryId)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT DATE(deu.StartTime) AS YYMMDD, SUM(CAST((strftime('%s', deu.EndTime) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu JOIN Devices d ON deu.DeviceId=d.Id
							            JOIN DeviceModels dm ON d.DeviceModelId=dm.Id
							            JOIN DeviceTypes dt ON dm.DeviceTypeId=dt.Id AND dt.CategoryId=@deviceCategoryId
							            JOIN Users u ON d.UserId=u.Id AND u.Id=@userId
                                        WHERE deu.StartTime >= date('now', '-1 month') 
                                            AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)
                                        GROUP BY DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@deviceCategoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@userId", userId));

                var energyUsages = new List<DailyEnergyConsumptionPastMonth>();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime date = DateTime.ParseExact(reader.GetString(0), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            var day = date.Day;
                            var month = date.ToString("MMMM");
                            var year = date.Year;
                            var energyUsage = Convert.ToDouble(reader.GetString(1));

                            var dailyEnergyUsage = new DailyEnergyConsumptionPastMonth
                            {
                                Day = day,
                                Month = month,
                                Year = year,
                                EnergyUsageResult = Math.Round(energyUsage, 2)
                            };

                            energyUsages.Add(dailyEnergyUsage);
                        }
                    }
                    else
                    {
                        var endDate = DateTime.Now.Date;
                        var startDate = endDate.AddMonths(-1);

                        for (var date = startDate; date <= endDate; date = date.AddDays(1))
                        {
                            var dailyEnergyUsage = new DailyEnergyConsumptionPastMonth
                            {
                                Day = date.Day,
                                Month = date.ToString("MMMM"),
                                Year = date.Year,
                                EnergyUsageResult = 0
                            };

                            energyUsages.Add(dailyEnergyUsage);
                        }
                    }
                }

                return energyUsages;
            }
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

        public List<EnergyToday> UserHistoryForThePastDayByHour(long userId, long deviceCategoryId)
        {
            var deviceIds = _context.Devices
                .Include(d => d.DeviceModel)
                .ThenInclude(dm => dm.DeviceType)
                .ThenInclude(dt => dt.DeviceCategory)
                .Where(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId)
                .Select(d => d.Id)
                .ToList();

            DateTime startOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day-1, 0, 0, 0);
            DateTime endOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day-1, 23, 59, 59);

            List<DeviceEnergyUsage> UsageList = new List<DeviceEnergyUsage>();
            var Results = new List<EnergyToday>();

            UsageList = _context.DeviceEnergyUsages
                        .Where(u => deviceIds.Contains(u.DeviceId) && u.StartTime >= startOfDay && (u.EndTime <= endOfDay || u.EndTime == null))
                        .ToList();

            for (var hour = startOfDay.Hour; hour <= endOfDay.Hour; hour = hour + 1)
            {
                var UsageForHour = UsageList.Where(u => u.StartTime.Hour == hour).ToList();

                double EnergyUsage = 0.0;
                foreach (var usage in UsageForHour)
                {
                    double EnergyInKwh = _context.Devices
                                        .Where(d => d.Id == usage.DeviceId)
                                        .Select(d => d.DeviceModel)
                                        .FirstOrDefault()
                                        .EnergyKwh;

                    EnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * EnergyInKwh;
                }

                Results.Add(new EnergyToday
                {
                    EnergyUsageResult = Math.Round(EnergyUsage, 2),
                    Hour = hour,
                    Day = startOfDay.Day,
                    Month = startOfDay.ToString("MMMM"),
                    Year = startOfDay.Year
                });
            }

            return Results;
        }

        public List<DailyEnergyConsumptionPastMonth> SettlementHistoryForThePastWeek(long settlementId, long deviceCategoryId)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT DATE(deu.StartTime) AS Day, 
                                               strftime('%m', deu.StartTime) AS Month,
                                               strftime('%Y', deu.StartTime) AS Year,
                                               SUM(CAST((strftime('%s', deu.EndTime) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu 
                                        JOIN Devices d ON deu.DeviceId = d.Id
                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
                                        JOIN Users u ON d.UserId = u.Id AND u.SettlementId = @settlementId
                                        WHERE deu.StartTime >= date('now', '-7 day') 
                                            AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)
                                        GROUP BY DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@settlementId", settlementId));

                var energyUsages = new List<DailyEnergyConsumptionPastMonth>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader.GetString(0), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var day = date.Day;
                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = Convert.ToDouble(reader.GetString(3));

                        var dailyEnergyUsage = new DailyEnergyConsumptionPastMonth
                        {
                            Day = day,
                            Month = month,
                            Year = year,
                            EnergyUsageResult = Math.Round(energyUsage, 2)
                        };

                        energyUsages.Add(dailyEnergyUsage);
                    }
                }

                return energyUsages;
            }
        }

        public List<DailyEnergyConsumptionPastMonth> CityHistoryForThePastWeek(long cityId, long deviceCategoryId)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT DATE(deu.StartTime) AS Day, 
                                               strftime('%m', deu.StartTime) AS Month,
                                               strftime('%Y', deu.StartTime) AS Year,
                                               SUM(CAST((strftime('%s', deu.EndTime) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu 
                                        JOIN Devices d ON deu.DeviceId = d.Id
                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
                                        JOIN Users u ON d.UserId = u.Id
                                        JOIN Settlements s ON s.Id = u.SettlementId AND s.CityId = @cityId
                                        WHERE deu.StartTime >= date('now', '-7 day') 
                                            AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)
                                        GROUP BY DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@cityId", cityId));

                var energyUsages = new List<DailyEnergyConsumptionPastMonth>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader.GetString(0), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var day = date.Day;
                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = Convert.ToDouble(reader.GetString(3));

                        var dailyEnergyUsage = new DailyEnergyConsumptionPastMonth
                        {
                            Day = day,
                            Month = month,
                            Year = year,
                            EnergyUsageResult = Math.Round(energyUsage, 2)
                        };

                        energyUsages.Add(dailyEnergyUsage);
                    }
                }

                return energyUsages;
            }
        }

        public List<DailyEnergyConsumptionPastMonth> SettlementHistoryForThePastMonth(long settlementId, long deviceCategoryId)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT DATE(deu.StartTime) AS Day, 
                                               strftime('%m', deu.StartTime) AS Month,
                                               strftime('%Y', deu.StartTime) AS Year,
                                               SUM(CAST((strftime('%s', deu.EndTime) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu 
                                        JOIN Devices d ON deu.DeviceId = d.Id
                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
                                        JOIN Users u ON d.UserId = u.Id AND u.SettlementId = @settlementId
                                        WHERE deu.StartTime >= date('now', '-1 month') 
                                            AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)
                                        GROUP BY DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@settlementId", settlementId));

                var energyUsages = new List<DailyEnergyConsumptionPastMonth>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader.GetString(0), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var day = date.Day;
                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = Convert.ToDouble(reader.GetString(3));

                        var dailyEnergyUsage = new DailyEnergyConsumptionPastMonth
                        {
                            Day = day,
                            Month = month,
                            Year = year,
                            EnergyUsageResult = Math.Round(energyUsage, 2)
                        };

                        energyUsages.Add(dailyEnergyUsage);
                    }
                }

                return energyUsages;
            }
        }

        public List<DailyEnergyConsumptionPastMonth> CityHistoryForThePastMonth(long cityId, long deviceCategoryId)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT DATE(deu.StartTime) AS Day, 
                                               strftime('%m', deu.StartTime) AS Month,
                                               strftime('%Y', deu.StartTime) AS Year,
                                               SUM(CAST((strftime('%s', deu.EndTime) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu 
                                        JOIN Devices d ON deu.DeviceId = d.Id
                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
                                        JOIN Users u ON d.UserId = u.Id
                                        JOIN Settlements s ON s.Id = u.SettlementId AND s.CityId = @cityId
                                        WHERE deu.StartTime >= date('now', '-1 month') 
                                            AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)
                                        GROUP BY DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@cityId", cityId));

                var energyUsages = new List<DailyEnergyConsumptionPastMonth>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader.GetString(0), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var day = date.Day;
                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = Convert.ToDouble(reader.GetString(3));

                        var dailyEnergyUsage = new DailyEnergyConsumptionPastMonth
                        {
                            Day = day,
                            Month = month,
                            Year = year,
                            EnergyUsageResult = Math.Round(energyUsage, 2)
                        };

                        energyUsages.Add(dailyEnergyUsage);
                    }
                }

                return energyUsages;
            }
        }

        public List<MonthlyEnergyConsumptionLastYear> CityHistoryForThePastYearByMonth(long cityId, long deviceCategoryId)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT strftime('%Y-%m', deu.StartTime) AS MonthYear, 
                                               SUM(CAST((strftime('%s', deu.EndTime) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu 
                                        JOIN Devices d ON deu.DeviceId=d.Id
							            JOIN DeviceModels dm ON d.DeviceModelId=dm.Id
							            JOIN DeviceTypes dt ON dm.DeviceTypeId=dt.Id AND dt.CategoryId = @categoryId
							            JOIN Users u ON d.UserId=u.Id
							            JOIN Settlements s ON s.Id=u.SettlementId AND s.CityId = @cityId
                                        WHERE deu.StartTime >= date('now', '-1 year') 
                                            AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)
                                        GROUP BY strftime('%Y-%m', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@cityId", cityId));

                var energyUsages = new List<MonthlyEnergyConsumptionLastYear>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader.GetString(0), "yyyy-MM", CultureInfo.InvariantCulture);

                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = reader.GetDouble(1);

                        var dailyEnergyUsage = new MonthlyEnergyConsumptionLastYear
                        {
                            Month = month,
                            Year = year,
                            EnergyUsageResult = Math.Round(energyUsage, 2)
                        };
                        energyUsages.Add(dailyEnergyUsage);
                    }
                }

                return energyUsages;
            }
        }

        public List<MonthlyEnergyConsumptionLastYear> SettlementHistoryForThePastYearByMonth(long settlementId, long deviceCategoryId)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT strftime('%Y-%m', deu.StartTime) AS MonthYear, 
                                               SUM(CAST((strftime('%s', deu.EndTime) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu 
                                        JOIN Devices d ON deu.DeviceId=d.Id
							            JOIN DeviceModels dm ON d.DeviceModelId=dm.Id
							            JOIN DeviceTypes dt ON dm.DeviceTypeId=dt.Id AND dt.CategoryId = @categoryId
							            JOIN Users u ON d.UserId=u.Id AND u.SettlementId = @settlementId
                                        WHERE deu.StartTime >= date('now', '-1 year') 
                                            AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)
                                        GROUP BY strftime('%Y-%m', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@settlementId", settlementId));

                var energyUsages = new List<MonthlyEnergyConsumptionLastYear>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader.GetString(0), "yyyy-MM", CultureInfo.InvariantCulture);

                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = reader.GetDouble(1);

                        var dailyEnergyUsage = new MonthlyEnergyConsumptionLastYear
                        {
                            Month = month,
                            Year = year,
                            EnergyUsageResult = Math.Round(energyUsage, 2)
                        };
                        energyUsages.Add(dailyEnergyUsage);
                    }
                }

                return energyUsages;
            }
        }

        public double GetUsageHistoryForDeviceInThisMonth(long deviceId)
        {
            DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            DateTime endTime = DateTime.Now;

            List<DeviceEnergyUsage> deviceEnergyUsageList = _context.DeviceEnergyUsages
                .Where(u => u.DeviceId == deviceId && u.StartTime >= startOfMonth && u.StartTime <= DateTime.Now/* && u.EndTime <= endTime*/)
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
                .Where(usage => usage.DeviceId == deviceId && usage.StartTime.Date >= startOfTheYear && usage.StartTime <= DateTime.Now)
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
