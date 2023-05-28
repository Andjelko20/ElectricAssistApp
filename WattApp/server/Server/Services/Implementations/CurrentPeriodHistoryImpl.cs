using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models;
using System.Globalization;

namespace Server.Services.Implementations
{
    public class CurrentPeriodHistoryImpl : ICurrentPeriodHistoryService
    {
        private readonly SqliteDbContext _context;
        public CurrentPeriodHistoryImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public double GetConsumptionForForwardedList(long deviceId, List<DeviceEnergyUsage> deviceEnergyUsageLista)
        {
            var Device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            var DeviceModel = _context.DeviceModels.FirstOrDefault(dm => dm.Id == Device.DeviceModelId);
            float EnergyInKwh = DeviceModel.EnergyKwh;

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

        
        public double GetUsageHistoryForProsumerFromCurrentDay(long userId, long deviceCategoryId)
        {
            DateTime StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            return SumEnergyConsumption(userId, deviceCategoryId, StartTime, EndTime);
        }

        public double GetUsageHistoryForProsumerFromCurrentMonth(long userId, long deviceCategoryId)
        {
            DateTime StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            DateTime EndTime = DateTime.Now;
            return SumEnergyConsumption(userId, deviceCategoryId, StartTime, EndTime);
        }

        public double GetUsageHistoryForProsumerFromCurrentYear(long userId, long deviceCategoryId)
        {
            DateTime StartTime = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
            DateTime EndTime = DateTime.Now;
            return SumEnergyConsumption(userId, deviceCategoryId, StartTime, EndTime);
        }

        public double SumEnergyConsumption(long userId, long deviceCategoryId, DateTime StartDate, DateTime EndDate)
        {
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

            usageList = _context.DeviceEnergyUsages
                        .Where(u => deviceIds.Contains(u.DeviceId) && u.StartTime >= StartDate && u.EndTime <= EndDate)
                        .ToList();

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

        // PAGINACIJA
        public List<EnergyToday> GetUsageHistoryForProsumerFromCurrentDayByHourPagination(long userId, long deviceCategoryId)
        {
            var deviceIds = _context.Devices
                .Include(d => d.DeviceModel)
                .ThenInclude(dm => dm.DeviceType)
                .ThenInclude(dt => dt.DeviceCategory)
                .Where(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId)
                .Select(d => d.Id)
                .ToList();

            DateTime startOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime endOfDay = DateTime.Now;

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

                    if (usage.EndTime == null)
                        usage.EndTime = DateTime.Now;

                    TimeSpan timeDifference = (TimeSpan)(usage.EndTime - usage.StartTime);
                    double hours = Math.Abs(timeDifference.TotalHours);
                    EnergyUsage += hours * EnergyInKwh;
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

        public List<EnergyToday> GetProsumerTodayByHourEnergyUsagePagination(long userId, long deviceCategoryId, int pageNumber, int itemsPerPage)
        {
            int skipCount = (pageNumber - 1) * itemsPerPage;

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT Datum, EnergyUsageKwh
                                        FROM (
	                                        SELECT strftime('%Y-%m-%d %H:00:00', deu.StartTime) AS Datum, 
		                                           SUM(CAST(
				                                        (strftime('%s', 
					                                        CASE 
						                                        WHEN deu.EndTime IS NULL 
						                                        THEN datetime('now', 'localtime')
						                                        ELSE deu.EndTime 
					                                        END
				                                        ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
			                                        ) AS EnergyUsageKwh,
		                                           ROW_NUMBER() OVER (ORDER BY DATE(deu.StartTime)) AS RowNumber
	                                        FROM DeviceEnergyUsages deu 
	                                        JOIN Devices d ON deu.DeviceId = d.Id AND d.UserId = @userId
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @deviceCategoryId
	                                        WHERE deu.StartTime >= datetime('now', 'start of day') 
		                                        AND (deu.EndTime <= datetime('now', 'localtime') OR deu.EndTime IS NULL)
	                                        GROUP BY strftime('%Y-%m-%d %H:00:00', deu.StartTime)
                                        ) AS T
                                        WHERE RowNumber > @skipCount
                                        LIMIT @itemsPerPage";

                command.Parameters.Add(new SqliteParameter("@userId", userId));
                command.Parameters.Add(new SqliteParameter("@deviceCategoryId", deviceCategoryId));
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
                    {
                        var endDate = DateTime.Now;
                        var startDate = endDate.AddHours(0);

                        for (var date = startDate; date <= endDate; date = date.AddDays(1))
                        {
                            var dailyEnergyUsage = new EnergyToday
                            {
                                Hour = date.Hour,
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

        public List<DailyEnergyConsumptionPastMonth> GetProsumerMonthByDayEnergyUsagePagination(long userId, long deviceCategoryId, int pageNumber, int itemsPerPage)
        {
            int skipCount = (pageNumber - 1) * itemsPerPage;

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT Datum, EnergyUsageKwh
                                        FROM (
	                                        SELECT strftime('%Y-%m-%d', deu.StartTime) AS Datum, 
		                                           SUM(CAST(
				                                        (strftime('%s', 
					                                        CASE 
						                                        WHEN deu.EndTime IS NULL 
						                                        THEN datetime('now', 'localtime')
						                                        ELSE deu.EndTime 
					                                        END
				                                        ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
			                                        ) AS EnergyUsageKwh,
		                                           ROW_NUMBER() OVER (ORDER BY DATE(deu.StartTime)) AS RowNumber
	                                        FROM DeviceEnergyUsages deu 
	                                        JOIN Devices d ON deu.DeviceId = d.Id AND d.UserId = @userId
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @deviceCategoryId
	                                        WHERE deu.StartTime >= datetime('now', 'start of month') 
		                                        AND (deu.EndTime <= datetime('now', 'localtime') OR deu.EndTime IS NULL)
	                                        GROUP BY strftime('%Y-%m-%d', deu.StartTime)
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
                    {
                        var endDate = DateTime.Now;
                        DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);

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

        public List<DailyEnergyConsumptionPastMonth> GetDeviceMonthByDayEnergyUsagePagination(long deviceId, int pageNumber, int itemsPerPage)
        {
            int skipCount = (pageNumber - 1) * itemsPerPage;

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT Datum, EnergyUsageKwh
                                        FROM (
	                                        SELECT strftime('%Y-%m-%d', deu.StartTime) AS Datum, 
		                                           SUM(CAST(
				                                        (strftime('%s', 
					                                        CASE 
						                                        WHEN deu.EndTime IS NULL 
						                                        THEN datetime('now', 'localtime')
						                                        ELSE deu.EndTime 
					                                        END
				                                        ) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh
			                                        ) AS EnergyUsageKwh,
		                                           ROW_NUMBER() OVER (ORDER BY DATE(deu.StartTime)) AS RowNumber
	                                        FROM DeviceEnergyUsages deu 
	                                        JOIN Devices d ON deu.DeviceId = d.Id AND d.Id = @deviceId
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        WHERE deu.StartTime >= datetime('now', 'start of month') 
		                                        AND (deu.EndTime <= datetime('now', 'localtime') OR deu.EndTime IS NULL)
	                                        GROUP BY strftime('%Y-%m-%d', deu.StartTime)
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
                    {
                        var endDate = DateTime.Now;
                        DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);

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
    }
}
