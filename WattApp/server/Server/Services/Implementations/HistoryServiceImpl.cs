using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Polly;
using Server.Data;
using Server.DTOs;
using Server.Models;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;
using System;
using System.Collections.Generic;
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
                if (energyUsage.EndTime == null)
                    energyUsage.EndTime = DateTime.Now;

                TimeSpan timeDifference = (TimeSpan)(energyUsage.EndTime - energyUsage.StartTime);
                double hours = Math.Abs(timeDifference.TotalHours);
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
            double Consumption = 0.0;
            double Hours = -1;
            foreach (var item in deviceEnergyUsageLista)
            {
                if (item.EndTime == null)
                    item.EndTime = DateTime.Now;

                TimeSpan timeDifference = (TimeSpan)(item.EndTime - item.StartTime);
                Hours = Math.Abs(timeDifference.TotalHours);
                Consumption += (double)(EnergyInKwh * Hours);
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
                        if (item.EndTime == null)
                            item.EndTime = DateTime.Now;

                        TimeSpan timeDifference = (TimeSpan)(item.EndTime - item.StartTime);
                        UsageInHours = Math.Abs(timeDifference.TotalHours);
                        UsageInKwh += UsageInHours * EnergyInKwh;
                    }
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
                {
                    if (usage.EndTime == null)
                        usage.EndTime = DateTime.Now;

                    TimeSpan timeDifference = (TimeSpan)(usage.EndTime - usage.StartTime);
                    double hours = Math.Abs(timeDifference.TotalHours);
                    EnergyUsage += hours * EnergyInKwh;
                }
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
                {
                    if (usage.EndTime == null)
                        usage.EndTime = DateTime.Now;

                    TimeSpan timeDifference = (TimeSpan)(usage.EndTime - usage.StartTime);
                    double hours = Math.Abs(timeDifference.TotalHours);
                    EnergyUsage += hours * EnergyInKwh;
                }

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
                    if (usage.EndTime == null)
                        usage.EndTime = DateTime.Now;

                    TimeSpan timeDifference = (TimeSpan)(usage.EndTime - usage.StartTime);
                    double hours = Math.Abs(timeDifference.TotalHours);
                    totalEnergyConsumption += hours * EnergyInKwh;
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
                        if (usage.EndTime == null)
                            usage.EndTime = DateTime.Now;
                        TimeSpan timeDifference = (TimeSpan)(usage.EndTime - usage.StartTime);
                        double hours = Math.Abs(timeDifference.TotalHours);
                        monthlyEnergyUsage += hours * EnergyInKwh;
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
                                        SELECT DATE(deu.StartTime) AS YYMMDD, SUM(CAST(
                                                                                        (strftime('%s', 
                                                                                            CASE 
                                                                                                WHEN deu.EndTime IS NULL 
                                                                                                THEN datetime('now', '+2 hour')
                                                                                                ELSE deu.EndTime 
                                                                                            END
                                                                                        ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                                                    ) AS EnergyUsageKwh
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
                            DateTime date = DateTime.ParseExact(reader["YYMMDD"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            var day = date.Day;
                            var month = date.ToString("MMMM");
                            var year = date.Year;
                            var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

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
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT DATE(deu.StartTime) AS YYMMDD, SUM(CAST(
                                                                                        (strftime('%s', 
                                                                                            CASE 
                                                                                                WHEN deu.EndTime IS NULL 
                                                                                                THEN datetime('now', '+2 hour')
                                                                                                ELSE deu.EndTime 
                                                                                            END
                                                                                        ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                                                    ) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu JOIN Devices d ON deu.DeviceId=d.Id
							            JOIN DeviceModels dm ON d.DeviceModelId=dm.Id
							            JOIN DeviceTypes dt ON dm.DeviceTypeId=dt.Id AND dt.CategoryId=@deviceCategoryId
							            JOIN Users u ON d.UserId=u.Id AND u.Id=@userId
                                        WHERE deu.StartTime >= date('now', '-7 days') 
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
                            DateTime date = DateTime.ParseExact(reader["YYMMDD"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            var day = date.Day;
                            var month = date.ToString("MMMM");
                            var year = date.Year;
                            var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

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
                        var startDate = endDate.AddDays(-7);

                        for (var date = startDate; date < endDate; date = date.AddDays(1))
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

        public List<EnergyToday> UserHistoryForThePastDayByHour(long userId, long deviceCategoryId)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime) AS Datum,
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', 'localtime')
								                                          THEN datetime('now', 'localtime')
								                                          WHEN deu.EndTime IS NULL THEN datetime('now', 'localtime')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id AND d.UserId = @userId
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
                                        WHERE
	                                        deu.StartTime >= datetime('now', '-1 day', 'start of day') AND deu.StartTime <= datetime('now', 'start of day', '-1 second')
                                        GROUP BY
	                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@userId", userId));

                var energyUsages = new List<EnergyToday>();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime date = DateTime.ParseExact(reader["Datum"].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                            var hour = date.Hour;
                            var day = date.Day;
                            var month = date.ToString("MMMM");
                            var year = date.Year;
                            var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

                            var dailyEnergyUsage = new EnergyToday
                            {
                                Hour = hour,
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
                        var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day-1, 0, 0, 0);
                        var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day-1, 23, 59, 59);

                        for (var date = startDate; date < endDate; date = date.AddHours(1))
                        {
                            var dailyEnergyUsage = new EnergyToday
                            {
                                Hour = date.Hour,
                                Day = date.Day,
                                Month = date.ToString("MMMM"),
                                Year = date.Year,
                                EnergyUsageResult = 0.0
                            };

                            energyUsages.Add(dailyEnergyUsage);
                        }
                    }
                }

                return energyUsages;
            }
        }

        public List<DailyEnergyConsumptionPastMonth> SettlementHistoryForThePastWeek(long settlementId, long deviceCategoryId)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT DATE(deu.StartTime) AS YYMMDD, 
                                               /*strftime('%m', deu.StartTime) AS Month,
                                               strftime('%Y', deu.StartTime) AS Year,*/
                                               SUM(CAST(
                                                    (strftime('%s', 
                                                        CASE 
                                                            WHEN deu.EndTime IS NULL 
                                                            THEN datetime('now', '+2 hour')
                                                            ELSE deu.EndTime 
                                                        END
                                                    ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                ) AS EnergyUsageKwh
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
                        DateTime date = DateTime.ParseExact(reader["YYMMDD"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var day = date.Day;
                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

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
                                        SELECT DATE(deu.StartTime) AS YYMMDD, 
                                               /*strftime('%m', deu.StartTime) AS Month,
                                               strftime('%Y', deu.StartTime) AS Year,*/
                                               SUM(CAST(
                                                    (strftime('%s', 
                                                        CASE 
                                                            WHEN deu.EndTime IS NULL 
                                                            THEN datetime('now', '+2 hour')
                                                            ELSE deu.EndTime 
                                                        END
                                                    ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                ) AS EnergyUsageKwh
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
                        DateTime date = DateTime.ParseExact(reader["YYMMDD"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var day = date.Day;
                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

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
                                        SELECT DATE(deu.StartTime) AS YYMMDD, 
                                               SUM(CAST(
                                                    (strftime('%s', 
                                                        CASE 
                                                            WHEN deu.EndTime IS NULL 
                                                            THEN datetime('now', '+2 hour')
                                                            ELSE deu.EndTime 
                                                        END
                                                    ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                ) AS EnergyUsageKwh
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
                        DateTime date = DateTime.ParseExact(reader["YYMMDD"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var day = date.Day;
                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

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
                                        SELECT DATE(deu.StartTime) AS YYMMDD, 
                                               SUM(CAST(
                                                    (strftime('%s', 
                                                        CASE 
                                                            WHEN deu.EndTime IS NULL 
                                                            THEN datetime('now', '+2 hour')
                                                            ELSE deu.EndTime 
                                                        END
                                                    ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                ) AS EnergyUsageKwh
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
                        DateTime date = DateTime.ParseExact(reader["YYMMDD"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var day = date.Day;
                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

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
                                               SUM(CAST(
                                                    (strftime('%s', 
                                                        CASE 
                                                            WHEN deu.EndTime IS NULL 
                                                            THEN datetime('now', '+2 hour')
                                                            ELSE deu.EndTime 
                                                        END
                                                    ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                ) AS EnergyUsageKwh
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
                        DateTime date = DateTime.ParseExact(reader["MonthYear"].ToString(), "yyyy-MM", CultureInfo.InvariantCulture);

                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

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
                                               SUM(CAST(
                                                    (strftime('%s', 
                                                        CASE 
                                                            WHEN deu.EndTime IS NULL 
                                                            THEN datetime('now', '+2 hour')
                                                            ELSE deu.EndTime 
                                                        END
                                                    ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                ) AS EnergyUsageKwh
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
                        DateTime date = DateTime.ParseExact(reader["MonthYear"].ToString(), "yyyy-MM", CultureInfo.InvariantCulture);

                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

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
            }

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageList);
        }

        public double GetUsageHistoryForDeviceToday(long deviceId)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT 
                                            SUM(CAST((strftime('%s', 
                                                CASE 
                                                    WHEN deu.EndTime IS NULL THEN datetime('now', 'localtime')
                                                    WHEN deu.EndTime > datetime('now', 'localtime') THEN datetime('now', 'localtime')
                                                    ELSE deu.EndTime 
                                                END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu
	                                         JOIN Devices d ON deu.DeviceId = d.Id AND deu.DeviceId = 94
	                                         JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                        WHERE deu.StartTime >= datetime('now', 'start of day') AND deu.StartTime < datetime('now', 'localtime')";

                command.Parameters.Add(new SqliteParameter("@deviceId", deviceId));

                double energyUsages = 0.0;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        energyUsages = double.Parse(reader["EnergyUsageKwh"].ToString());
                    }
                }

                return energyUsages;
            }
        }

        public double GetUsageHistoryForDeviceThisYear(long deviceId)
        {
            DateTime startOfTheYear = new DateTime(DateTime.Now.Year, 1, 1);
            // za trazeni uredjaj, samo kada je radio od pocetka ove godine
            var deviceEnergyUsages = _context.DeviceEnergyUsages
                .Where(usage => usage.DeviceId == deviceId && usage.StartTime.Date >= startOfTheYear && usage.StartTime <= DateTime.Now)
                .ToList();

            foreach (var usage in deviceEnergyUsages)
            {
                // od 01.01.2023.(trenutne godine) 00:00:00h do ovog trenutka, danasnjeg dana
                if (usage.EndTime > DateTime.Now)
                    usage.EndTime = DateTime.Now;
            }

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsages);
        }

        public double GetUsageHistoryForDeviceForPreviousMonth(long deviceId)
        {
            DateTime startOfThePreviousMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
            DateTime startOfTheCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endOfThePreviousMonth = startOfTheCurrentMonth.AddSeconds(-1);
            // za trazeni uredjaj, samo kada je radio od pocetka do kraja prethodnog meseca
            var deviceEnergyUsages = _context.DeviceEnergyUsages
                .Where(usage => usage.DeviceId == deviceId && usage.StartTime.Date >= startOfThePreviousMonth && usage.EndTime <= endOfThePreviousMonth)
                .ToList();

            // foreach (var usage in deviceEnergyUsages)
            // {
            // od 01. u prethodnom mesecu od 00:00:00h do 23:59:59h poslednjeg dana u mesecu
            /*if (usage.EndTime > endOfThePreviousMonth)
                usage.EndTime = endOfThePreviousMonth;*/
            // }

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsages);
        }

        // PAGINACIJA
        public List<DailyEnergyConsumptionPastMonth> GetDailyEnergyUsageForPastMonthPagination(long deviceId, int pageNumber, int itemsPerPage)
        {
            int skipCount = (pageNumber - 1) * itemsPerPage;

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT Datum, EnergyUsageKwh
                                        FROM (
                                            SELECT DATE(deu.StartTime) AS Datum, 
                                                   SUM(CAST(
                                                        (strftime('%s', 
                                                            CASE 
                                                                WHEN deu.EndTime IS NULL 
                                                                THEN datetime('now', '+2 hour')
                                                                ELSE deu.EndTime 
                                                            END
                                                        ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                    ) AS EnergyUsageKwh,
                                                   ROW_NUMBER() OVER (ORDER BY DATE(deu.StartTime)) AS RowNumber
                                            FROM DeviceEnergyUsages deu 
                                            JOIN Devices d ON deu.DeviceId = d.Id AND d.Id = @deviceId
                                            JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                            WHERE deu.StartTime >= date('now', '-1 month') 
                                                AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)
                                            GROUP BY DATE(deu.StartTime)
                                        ) AS T
                                        WHERE RowNumber > @skipCount
                                        LIMIT @itemsPerPage";

                command.Parameters.Add(new SqliteParameter("@deviceId", deviceId));
                command.Parameters.Add(new SqliteParameter("@skipCount", skipCount));
                command.Parameters.Add(new SqliteParameter("@itemsPerPage", itemsPerPage));

                var energyUsages = new List<DailyEnergyConsumptionPastMonth>();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime date = DateTime.ParseExact(reader["Datum"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            var day = date.Day;
                            var month = date.ToString("MMMM");
                            var year = date.Year;
                            var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

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
                        FillInWithZerosConsumptionProductionMonthByDay(skipCount, itemsPerPage, energyUsages);
                }

                return energyUsages;
            }
        }

        public List<DailyEnergyConsumptionPastMonth> GetProsumerDailyEnergyUsageForPastMonthPagination(long userId, long deviceCategoryId, int pageNumber, int itemsPerPage)
        {
            int skipCount = (pageNumber - 1) * itemsPerPage;

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT Datum, EnergyUsageKwh
                                        FROM (
                                            SELECT DATE(deu.StartTime) AS Datum, 
                                                   SUM(CAST(
                                                        (strftime('%s', 
                                                            CASE 
                                                                WHEN deu.EndTime IS NULL 
                                                                THEN datetime('now', '+2 hour')
                                                                ELSE deu.EndTime 
                                                            END
                                                        ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                    ) AS EnergyUsageKwh,
                                                   ROW_NUMBER() OVER (ORDER BY DATE(deu.StartTime)) AS RowNumber
                                            FROM DeviceEnergyUsages deu 
                                            JOIN Devices d ON deu.DeviceId = d.Id AND d.UserId = @userId
                                            JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                            JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @deviceCategoryId
                                            WHERE deu.StartTime >= date('now', '-1 month') 
                                                AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)
                                            GROUP BY DATE(deu.StartTime)
                                        ) AS T
                                        WHERE RowNumber > @skipCount
                                        LIMIT @itemsPerPage";

                command.Parameters.Add(new SqliteParameter("@userId", userId));
                command.Parameters.Add(new SqliteParameter("@deviceCategoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@skipCount", skipCount));
                command.Parameters.Add(new SqliteParameter("@itemsPerPage", itemsPerPage));

                var energyUsages = new List<DailyEnergyConsumptionPastMonth>();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime date = DateTime.ParseExact(reader["Datum"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            var day = date.Day;
                            var month = date.ToString("MMMM");
                            var year = date.Year;
                            var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

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
                        FillInWithZerosConsumptionProductionMonthByDay(skipCount, itemsPerPage, energyUsages);
                }

                return energyUsages;
            }
        }

        public List<DailyEnergyConsumptionPastMonth> GetSettlementDailyEnergyUsageForPastMonthPagination(long settlementId, long deviceCategoryId, int pageNumber, int itemsPerPage)
        {
            int skipCount = (pageNumber - 1) * itemsPerPage;

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT Datum, EnergyUsageKwh
                                        FROM (
                                            SELECT DATE(deu.StartTime) AS Datum, 
                                                   SUM(CAST(
                                                        (strftime('%s', 
                                                            CASE 
                                                                WHEN deu.EndTime IS NULL 
                                                                THEN datetime('now', '+2 hour')
                                                                ELSE deu.EndTime 
                                                            END
                                                        ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                    ) AS EnergyUsageKwh,
                                                   ROW_NUMBER() OVER (ORDER BY DATE(deu.StartTime)) AS RowNumber
                                            FROM DeviceEnergyUsages deu 
                                            JOIN Devices d ON deu.DeviceId = d.Id
                                            JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                            JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @deviceCategoryId
                                            JOIN Users u ON d.UserId = u.Id AND u.SettlementId = @settlementId
                                            WHERE deu.StartTime >= date('now', '-1 month') 
                                                AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)
                                            GROUP BY DATE(deu.StartTime)
                                        ) AS T
                                        WHERE RowNumber > @skipCount
                                        LIMIT @itemsPerPage";

                command.Parameters.Add(new SqliteParameter("@settlementId", settlementId));
                command.Parameters.Add(new SqliteParameter("@deviceCategoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@skipCount", skipCount));
                command.Parameters.Add(new SqliteParameter("@itemsPerPage", itemsPerPage));

                var energyUsages = new List<DailyEnergyConsumptionPastMonth>();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime date = DateTime.ParseExact(reader["Datum"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            var day = date.Day;
                            var month = date.ToString("MMMM");
                            var year = date.Year;
                            var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

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
                        FillInWithZerosConsumptionProductionMonthByDay(skipCount, itemsPerPage, energyUsages);
                }

                return energyUsages;
            }
        }

        public List<DailyEnergyConsumptionPastMonth> GetCityDailyEnergyUsageForPastMonthPagination(long cityId, long deviceCategoryId, int pageNumber, int itemsPerPage)
        {
            int skipCount = (pageNumber - 1) * itemsPerPage;

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT Datum, EnergyUsageKwh
                                        FROM (
                                            SELECT DATE(deu.StartTime) AS Datum, 
                                                   SUM(CAST(
                                                        (strftime('%s', 
                                                            CASE 
                                                                WHEN deu.EndTime IS NULL 
                                                                THEN datetime('now', '+2 hour')
                                                                ELSE deu.EndTime 
                                                            END
                                                        ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                    ) AS EnergyUsageKwh,
                                                   ROW_NUMBER() OVER (ORDER BY DATE(deu.StartTime)) AS RowNumber
                                            FROM DeviceEnergyUsages deu 
                                            JOIN Devices d ON deu.DeviceId = d.Id
                                            JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                            JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @deviceCategoryId
                                            JOIN Users u ON d.UserId = u.Id
                                            JOIN Settlements s ON s.Id = u.SettlementId AND s.CityId = @cityId
                                            WHERE deu.StartTime >= date('now', '-1 month') 
                                                AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)
                                            GROUP BY DATE(deu.StartTime)
                                        ) AS T
                                        WHERE RowNumber > @skipCount
                                        LIMIT @itemsPerPage";

                command.Parameters.Add(new SqliteParameter("@cityId", cityId));
                command.Parameters.Add(new SqliteParameter("@deviceCategoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@skipCount", skipCount));
                command.Parameters.Add(new SqliteParameter("@itemsPerPage", itemsPerPage));

                var energyUsages = new List<DailyEnergyConsumptionPastMonth>();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime date = DateTime.ParseExact(reader["Datum"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            var day = date.Day;
                            var month = date.ToString("MMMM");
                            var year = date.Year;
                            var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

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
                        FillInWithZerosConsumptionProductionMonthByDay(skipCount, itemsPerPage, energyUsages);
                }

                return energyUsages;
            }
        }

        public List<EnergyToday> UserHistoryForThePastDayByHourPagination(long userId, long deviceCategoryId, int pageNumber, int itemsPerPage)
        {
            int skipCount = (pageNumber - 1) * itemsPerPage;
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT Datum, EnergyUsageKwh
                                        FROM (
	                                        SELECT
		                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime) AS Datum,
		                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', 'localtime')
									                                          THEN datetime('now', 'localtime')
									                                          WHEN deu.EndTime IS NULL THEN datetime('now', 'localtime')
									                                          ELSE deu.EndTime
								                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh,
		                                        ROW_NUMBER() OVER (ORDER BY DATE(deu.StartTime)) AS RowNumber
	                                        FROM
		                                        DeviceEnergyUsages deu
		                                        JOIN Devices d ON deu.DeviceId = d.Id AND d.UserId = @userId
		                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
		                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
	                                        WHERE
		                                        deu.StartTime >= datetime('now', '-1 day', 'start of day') AND deu.StartTime <= datetime('now', 'start of day', '-1 second')
	                                        GROUP BY
		                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime)
	                                        ) AS T
                                        WHERE RowNumber > @skipCount
                                        LIMIT @itemsPerPage";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@userId", userId));
                command.Parameters.Add(new SqliteParameter("@skipCount", skipCount));
                command.Parameters.Add(new SqliteParameter("@itemsPerPage", itemsPerPage));

                var energyUsages = new List<EnergyToday>();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime date = DateTime.ParseExact(reader["Datum"].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                            var hour = date.Hour;
                            var day = date.Day;
                            var month = date.ToString("MMMM");
                            var year = date.Year;
                            var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

                            var dailyEnergyUsage = new EnergyToday
                            {
                                Hour = hour,
                                Day = day,
                                Month = month,
                                Year = year,
                                EnergyUsageResult = Math.Round(energyUsage, 2)
                            };

                            energyUsages.Add(dailyEnergyUsage);
                        }
                    }
                    else
                        FillInWithZerosConsumptionProductionDayByHour(skipCount, itemsPerPage, energyUsages);
                }

                return energyUsages;
            }
        }

        // YEAR
        public List<MonthlyEnergyConsumptionLastYear> CityHistoryForYearByMonth(long cityId, long deviceCategoryId, int yearNumber)
        {
            DateTime fromDate = new DateTime(yearNumber, 1, 1);
            DateTime toDate = new DateTime(yearNumber, 12, 31);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT strftime('%Y-%m', deu.StartTime) AS MonthYear, 
                                               SUM(CAST(
                                                    (strftime('%s', 
                                                        CASE 
                                                            WHEN deu.EndTime IS NULL 
                                                            THEN datetime('now', '+2 hour')
                                                            ELSE deu.EndTime 
                                                        END
                                                    ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                ) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu 
                                        JOIN Devices d ON deu.DeviceId=d.Id
							            JOIN DeviceModels dm ON d.DeviceModelId=dm.Id
							            JOIN DeviceTypes dt ON dm.DeviceTypeId=dt.Id AND dt.CategoryId = @categoryId
							            JOIN Users u ON d.UserId=u.Id
							            JOIN Settlements s ON s.Id=u.SettlementId AND s.CityId = @cityId
                                        WHERE deu.StartTime >= @fromDate AND deu.StartTime < @toDate
                                            AND (deu.EndTime <= @toDate OR deu.EndTime IS NULL)
                                        GROUP BY strftime('%Y-%m', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@cityId", cityId));
                command.Parameters.Add(new SqliteParameter("@fromDate", fromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", toDate));

                var energyUsages = new List<MonthlyEnergyConsumptionLastYear>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader["MonthYear"].ToString(), "yyyy-MM", CultureInfo.InvariantCulture);

                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

                        var dailyEnergyUsage = new MonthlyEnergyConsumptionLastYear
                        {
                            Month = month,
                            Year = year,
                            EnergyUsageResult = Math.Round(energyUsage, 2)
                        };
                        energyUsages.Add(dailyEnergyUsage);
                    }
                }

                for (int i = 1; i <= 12; i++)
                {
                    if (!energyUsages.Any(x => DateTime.ParseExact(x.Month, "MMMM", CultureInfo.InvariantCulture).Month == i))
                    {
                        var emptyMonth = new DateTime(yearNumber, i, 1).ToString("MMMM");
                        var monthlyEnergyUsage = new MonthlyEnergyConsumptionLastYear
                        {
                            Month = emptyMonth,
                            Year = yearNumber,
                            EnergyUsageResult = 0
                        };
                        energyUsages.Add(monthlyEnergyUsage);
                    }
                }

                return energyUsages.OrderBy(x => DateTime.ParseExact(x.Month, "MMMM", CultureInfo.InvariantCulture).Month).ToList();
            }
        }

        public List<MonthlyEnergyConsumptionLastYear> SettlementHistoryForYearByMonth(long settlementId, long deviceCategoryId, int yearNumber)
        {
            DateTime fromDate = new DateTime(yearNumber, 1, 1);
            DateTime toDate = new DateTime(yearNumber, 12, 31);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT strftime('%Y-%m', deu.StartTime) AS MonthYear, 
                                               SUM(CAST(
                                                    (strftime('%s', 
                                                        CASE 
                                                            WHEN deu.EndTime IS NULL 
                                                            THEN datetime('now', '+2 hour')
                                                            ELSE deu.EndTime 
                                                        END
                                                    ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                ) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu 
                                        JOIN Devices d ON deu.DeviceId=d.Id
							            JOIN DeviceModels dm ON d.DeviceModelId=dm.Id
							            JOIN DeviceTypes dt ON dm.DeviceTypeId=dt.Id AND dt.CategoryId = @categoryId
							            JOIN Users u ON d.UserId=u.Id AND u.SettlementId = @settlementId
                                        WHERE deu.StartTime >= @fromDate AND deu.StartTime < @toDate
                                            AND (deu.EndTime <= @toDate OR deu.EndTime IS NULL)
                                        GROUP BY strftime('%Y-%m', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@settlementId", settlementId));
                command.Parameters.Add(new SqliteParameter("@fromDate", fromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", toDate));

                var energyUsages = new List<MonthlyEnergyConsumptionLastYear>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader["MonthYear"].ToString(), "yyyy-MM", CultureInfo.InvariantCulture);

                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

                        var dailyEnergyUsage = new MonthlyEnergyConsumptionLastYear
                        {
                            Month = month,
                            Year = year,
                            EnergyUsageResult = Math.Round(energyUsage, 2)
                        };
                        energyUsages.Add(dailyEnergyUsage);
                    }
                }

                for (int i = 1; i <= 12; i++)
                {
                    if (!energyUsages.Any(x => DateTime.ParseExact(x.Month, "MMMM", CultureInfo.InvariantCulture).Month == i))
                    {
                        var emptyMonth = new DateTime(yearNumber, i, 1).ToString("MMMM");
                        var monthlyEnergyUsage = new MonthlyEnergyConsumptionLastYear
                        {
                            Month = emptyMonth,
                            Year = yearNumber,
                            EnergyUsageResult = 0
                        };
                        energyUsages.Add(monthlyEnergyUsage);
                    }
                }

                return energyUsages.OrderBy(x => DateTime.ParseExact(x.Month, "MMMM", CultureInfo.InvariantCulture).Month).ToList();
            }
        }

        public List<MonthlyEnergyConsumptionLastYear> UserHistoryForYearByMonth(long userId, long deviceCategoryId, int yearNumber)
        {
            DateTime fromDate = new DateTime(yearNumber, 1, 1);
            DateTime toDate = new DateTime(yearNumber, 12, 31);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT strftime('%Y-%m', deu.StartTime) AS MonthYear, 
                                               SUM(CAST(
                                                    (strftime('%s', 
                                                        CASE 
                                                            WHEN deu.EndTime IS NULL 
                                                            THEN datetime('now', '+2 hour')
                                                            ELSE deu.EndTime 
                                                        END
                                                    ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                ) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu 
                                        JOIN Devices d ON deu.DeviceId=d.Id AND d.UserId = @userId
							            JOIN DeviceModels dm ON d.DeviceModelId=dm.Id
							            JOIN DeviceTypes dt ON dm.DeviceTypeId=dt.Id AND dt.CategoryId = @categoryId
                                        WHERE deu.StartTime >= @fromDate AND deu.StartTime < @toDate
                                            AND (deu.EndTime <= @toDate OR deu.EndTime IS NULL)
                                        GROUP BY strftime('%Y-%m', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@userId", userId));
                command.Parameters.Add(new SqliteParameter("@fromDate", fromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", toDate));

                var energyUsages = new List<MonthlyEnergyConsumptionLastYear>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader["MonthYear"].ToString(), "yyyy-MM", CultureInfo.InvariantCulture);

                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

                        var dailyEnergyUsage = new MonthlyEnergyConsumptionLastYear
                        {
                            Month = month,
                            Year = year,
                            EnergyUsageResult = Math.Round(energyUsage, 2)
                        };
                        energyUsages.Add(dailyEnergyUsage);
                    }
                }

                for (int i = 1; i <= 12; i++)
                {
                    if (!energyUsages.Any(x => DateTime.ParseExact(x.Month, "MMMM", CultureInfo.InvariantCulture).Month == i))
                    {
                        var emptyMonth = new DateTime(yearNumber, i, 1).ToString("MMMM");
                        var monthlyEnergyUsage = new MonthlyEnergyConsumptionLastYear
                        {
                            Month = emptyMonth,
                            Year = yearNumber,
                            EnergyUsageResult = 0
                        };
                        energyUsages.Add(monthlyEnergyUsage);
                    }
                }

                return energyUsages.OrderBy(x => DateTime.ParseExact(x.Month, "MMMM", CultureInfo.InvariantCulture).Month).ToList();
            }
        }

        public List<MonthlyEnergyConsumptionLastYear> DeviceHistoryForYearByMonth(long deviceId, int yearNumber)
        {
            DateTime fromDate = new DateTime(yearNumber, 1, 1);
            DateTime toDate = new DateTime(yearNumber, 12, 31);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT strftime('%Y-%m', deu.StartTime) AS MonthYear, 
                                               SUM(CAST(
                                                    (strftime('%s', 
                                                        CASE 
                                                            WHEN deu.EndTime IS NULL 
                                                            THEN datetime('now', '+2 hour')
                                                            ELSE deu.EndTime 
                                                        END
                                                    ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
                                                ) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu 
                                        JOIN Devices d ON deu.DeviceId=d.Id AND deu.DeviceId = @deviceId
							            JOIN DeviceModels dm ON d.DeviceModelId=dm.Id
                                        WHERE deu.StartTime >= @fromDate AND deu.StartTime < @toDate
                                            AND (deu.EndTime <= @toDate OR deu.EndTime IS NULL)
                                        GROUP BY strftime('%Y-%m', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@deviceId", deviceId));
                command.Parameters.Add(new SqliteParameter("@fromDate", fromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", toDate));

                var energyUsages = new List<MonthlyEnergyConsumptionLastYear>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader["MonthYear"].ToString(), "yyyy-MM", CultureInfo.InvariantCulture);

                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

                        var dailyEnergyUsage = new MonthlyEnergyConsumptionLastYear
                        {
                            Month = month,
                            Year = year,
                            EnergyUsageResult = Math.Round(energyUsage, 2)
                        };
                        energyUsages.Add(dailyEnergyUsage);
                    }
                }

                for (int i = 1; i <= 12; i++)
                {
                    if (!energyUsages.Any(x => DateTime.ParseExact(x.Month, "MMMM", CultureInfo.InvariantCulture).Month == i))
                    {
                        var emptyMonth = new DateTime(yearNumber, i, 1).ToString("MMMM");
                        var monthlyEnergyUsage = new MonthlyEnergyConsumptionLastYear
                        {
                            Month = emptyMonth,
                            Year = yearNumber,
                            EnergyUsageResult = 0
                        };
                        energyUsages.Add(monthlyEnergyUsage);
                    }
                }

                return energyUsages.OrderBy(x => DateTime.ParseExact(x.Month, "MMMM", CultureInfo.InvariantCulture).Month).ToList();
            }
        }

        public void FillInWithZerosConsumptionProductionMonthByDay(int skipCount, int itemsPerPage, List<DailyEnergyConsumptionPastMonth> energyUsages)
        {
            var endDate = DateTime.Now.Date;
            var startDate = endDate.AddMonths(-1);

            int itemsAdded = 0;
            for (var date = startDate; date < endDate; date = date.AddDays(1))
            {
                if (itemsAdded >= skipCount && itemsAdded < skipCount + itemsPerPage)
                {
                    var dailyEnergyUsage = new DailyEnergyConsumptionPastMonth
                    {
                        Day = date.Day,
                        Month = date.ToString("MMMM"),
                        Year = date.Year,
                        EnergyUsageResult = 0.0
                    };

                    energyUsages.Add(dailyEnergyUsage);
                }

                itemsAdded++;
                if (itemsAdded >= skipCount + itemsPerPage)
                    break;
            }
        }

        public void FillInWithZerosConsumptionProductionDayByHour(int skipCount, int itemsPerPage, List<EnergyToday> energyUsages)
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var endDate = DateTime.Now;

            int itemsAdded = 0;
            for (var date = startDate; date < endDate; date = date.AddHours(1))
            {
                if (itemsAdded >= skipCount && itemsAdded < skipCount + itemsPerPage)
                {
                    var dailyEnergyUsage = new EnergyToday
                    {
                        Hour = date.Hour,
                        Day = date.Day,
                        Month = date.ToString("MMMM"),
                        Year = date.Year,
                        EnergyUsageResult = 0.0
                    };

                    energyUsages.Add(dailyEnergyUsage);
                }

                itemsAdded++;
                if (itemsAdded >= skipCount + itemsPerPage)
                    break;
            }
        }
    }
}
