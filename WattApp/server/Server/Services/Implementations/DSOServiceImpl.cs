using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;
using System.Collections.Generic;
using System.Globalization;

namespace Server.Services.Implementations
{
    public class DSOServiceImpl : IDSOService
    {
        private readonly SqliteDbContext _context;
        public DSOServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public long GetCityId(string cityName)
        {
            var city = _context.Cities.FirstOrDefault(c => EF.Functions.Like(c.Name, cityName));

            if (city == null) 
                return -1;
            return city.Id;
        }

        public List<SettlementDTO> GetSettlements(long cityId)
        {
            var settlements = _context.Settlements.Where(s => s.CityId == cityId).Select(s => new SettlementDTO { Id=s.Id, Name=s.Name }).ToList();
            return settlements;
        }

        public double GetCityConsumptionForToday(long cityId, long deviceCategoryId)
        {
            var now = DateTime.Now;
            var totalUsage = 0.0;

            // pronalazimo sve tipove uredjaja koji pripadaju toj kategoriji
            var deviceTypeIds = _context.DeviceTypes
                .Where(dt => dt.CategoryId == deviceCategoryId)
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

            // pronalazimo uredjaje te kategorije u tabeli DeviceEnergyUsages i to ako trenutno rade
            var usages = _context.DeviceEnergyUsages
                            .Where(u => devices.Contains(u.DeviceId)
                                    && u.StartTime <= now
                                    && _context.Devices.Any(d => d.Id == u.DeviceId
                                        && _context.Users.Any(u => u.Id == d.UserId
                                            && _context.Settlements.Any(s => s.Id == u.SettlementId
                                                && s.CityId == cityId
                                                && _context.Cities.Any(c => c.Id == s.CityId)))))
                            .Where(u => u.StartTime >= now.Date)
                            .ToList();

            foreach (var usage in usages)
            {
                var usageStart = usage.StartTime;

                var usageEnd = usage.EndTime;
                if (usageEnd == null || usageEnd > now)
                {
                    usageEnd = now;
                }

                TimeSpan timeDifference = (TimeSpan)(usageEnd - usage.StartTime);
                double usageTime = Math.Abs(timeDifference.TotalHours);
                var deviceEnergyUsage = _context.Devices
                    .Include(d => d.DeviceModel)
                    .Where(d => d.Id == usage.DeviceId)
                    .Select(d => d.DeviceModel.EnergyKwh)
                    .FirstOrDefault();

                totalUsage += deviceEnergyUsage * usageTime;
            }

            return Math.Round(totalUsage, 2);
            //return totalUsage;
        }

        public double GetUsageHistoryForDeviceInThisMonth(long cityId, long deviceCategoryId)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT SUM(CAST((strftime('%s', deu.EndTime) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL)*dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu JOIN Devices d ON deu.DeviceId=d.Id
							            JOIN DeviceModels dm ON d.DeviceModelId=dm.Id
							            JOIN DeviceTypes dt ON dm.DeviceTypeId=dt.Id AND dt.CategoryId = @categoryId
							            JOIN Users u ON d.UserId=u.Id
							            JOIN Settlements s ON s.Id = u.SettlementId AND s.CityId = @cityId
                                        WHERE deu.StartTime >= date('now', 'start of month') 
                                            AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@cityId", cityId));

                double energyUsage = 0;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        energyUsage += double.Parse(reader["EnergyUsageKwh"].ToString());
                    }
                }
                return Math.Round(energyUsage, 2);
            }
        }

        public double GetUsageHistoryForDeviceInThisYear(long cityId, long deviceCategoryId)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT SUM(CAST((strftime('%s', deu.EndTime) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL)*dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu JOIN Devices d ON deu.DeviceId=d.Id
							            JOIN DeviceModels dm ON d.DeviceModelId=dm.Id
							            JOIN DeviceTypes dt ON dm.DeviceTypeId=dt.Id AND dt.CategoryId = @categoryId
							            JOIN Users u ON d.UserId=u.Id
							            JOIN Settlements s ON s.Id = u.SettlementId AND s.CityId = @cityId
                                        WHERE deu.StartTime >= date('now', 'start of year') 
                                            AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@cityId", cityId));

                double energyUsage = 0;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        energyUsage += double.Parse(reader["EnergyUsageKwh"].ToString());
                    }
                }
                return Math.Round(energyUsage, 2);
            }
        }

        public List<EnergyToday> CalculateSettlementEnergyUsageForToday(long settlementId, long deviceCategoryId)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
                                            strftime('%H', deu.StartTime) AS Hour,
                                            strftime('%d', deu.StartTime) AS Day,
                                            strftime('%m', deu.StartTime) AS Month,
                                            strftime('%Y', deu.StartTime) AS Year,
                                            SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', 'localtime')
                                                                          THEN datetime('now', 'localtime')
                                                                          ELSE deu.EndTime
                                                                     END) - strftime('%s', CASE WHEN deu.StartTime < datetime('now', 'start of day')
                                                                                               THEN datetime('now', 'start of day')
                                                                                               ELSE deu.StartTime
                                                                                          END)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
                                            DeviceEnergyUsages deu
                                            JOIN Devices d ON deu.DeviceId = d.Id
                                            JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                            JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
                                            JOIN Users u ON d.UserId = u.Id AND u.SettlementId = @settlementId
                                        WHERE
                                            deu.StartTime >= datetime('now', 'start of day')
                                            AND deu.StartTime <= datetime('now', 'localtime')
                                        GROUP BY
                                            strftime('%H', deu.StartTime),
                                            strftime('%d', deu.StartTime),
                                            strftime('%m', deu.StartTime),
                                            strftime('%Y', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@settlementId", settlementId));

                var energyUsages = new List<EnergyToday>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTimeFormatInfo dateFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
                        string monthName = dateFormatInfo.GetMonthName(int.Parse(reader["Month"].ToString()));

                        var hour = int.Parse(reader["Hour"].ToString());
                        var day = int.Parse(reader["Day"].ToString());
                        var month = monthName;
                        var year = int.Parse(reader["Year"].ToString());
                        var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

                        var dailyEnergyUsage = new EnergyToday
                        {
                            EnergyUsageResult = Math.Round(energyUsage, 2),
                            Hour = hour,
                            Day = day,
                            Month = month,
                            Year = year
                        };

                        energyUsages.Add(dailyEnergyUsage);
                    }
                }

                return energyUsages;
            }
        }

        public List<EnergyToday> CalculateEnergyUsageForTodayInCity(long cityId, long deviceCategoryId)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
                                            strftime('%H', deu.StartTime) AS Hour,
                                            strftime('%d', deu.StartTime) AS Day,
                                            strftime('%m', deu.StartTime) AS Month,
                                            strftime('%Y', deu.StartTime) AS Year,
                                            SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', 'localtime')
                                                                          THEN datetime('now', 'localtime')
                                                                          ELSE deu.EndTime
                                                                     END) - strftime('%s', CASE WHEN deu.StartTime < datetime('now', 'start of day')
                                                                                               THEN datetime('now', 'start of day')
                                                                                               ELSE deu.StartTime
                                                                                          END)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
                                            DeviceEnergyUsages deu
                                            JOIN Devices d ON deu.DeviceId = d.Id
                                            JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                            JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
                                            JOIN Users u ON d.UserId = u.Id
                                            JOIN Settlements s ON s.Id = u.SettlementId AND s.CityId = @cityId
                                        WHERE
                                            deu.StartTime >= datetime('now', 'start of day')
                                            AND deu.StartTime <= datetime('now', 'localtime')
                                        GROUP BY
                                            strftime('%H', deu.StartTime),
                                            strftime('%d', deu.StartTime),
                                            strftime('%m', deu.StartTime),
                                            strftime('%Y', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@cityId", cityId));

                var energyUsages = new List<EnergyToday>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTimeFormatInfo dateFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
                        string monthName = dateFormatInfo.GetMonthName(int.Parse(reader["Month"].ToString()));

                        var hour = int.Parse(reader["Hour"].ToString());
                        var day = int.Parse(reader["Day"].ToString());
                        var month = monthName;
                        var year = int.Parse(reader["Year"].ToString());
                        var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

                        var dailyEnergyUsage = new EnergyToday
                        {
                            EnergyUsageResult = Math.Round(energyUsage, 2),
                            Hour = hour,
                            Day = day,
                            Month = month,
                            Year = year
                        };

                        energyUsages.Add(dailyEnergyUsage);
                    }
                }

                return energyUsages;
            }
        }

        public List<EnergyToday> GetCityHistoryTodayByHourPagination(long cityId, long deviceCategoryId, int pageNumber, int itemsPerPage)
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
	                                            JOIN Devices d ON deu.DeviceId = d.Id
	                                            JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                                JOIN DeviceTypes dt ON dt.Id = dm.DeviceTypeId AND dt.CategoryId = @deviceCategoryId
                                                JOIN Users u ON u.Id = d.UserId
                                                JOIN Settlements s ON s.Id = u.SettlementId AND s.CityId = @cityId
                                            WHERE
	                                            deu.StartTime >= datetime('now', 'start of day') AND deu.StartTime <= datetime('now', 'localtime')
                                            GROUP BY
	                                            strftime('%Y-%m-%d %H:00:00', deu.StartTime)
                                        ) AS T
                                        WHERE RowNumber > @skipCount
                                        LIMIT @itemsPerPage";

                command.Parameters.Add(new SqliteParameter("@cityId", cityId));
                command.Parameters.Add(new SqliteParameter("@deviceCategoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@skipCount", skipCount));
                command.Parameters.Add(new SqliteParameter("@itemsPerPage", itemsPerPage));

                var energyUsages = new List<EnergyToday>();

                using (var reader = command.ExecuteReader())
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

                return energyUsages;
            }
        }

        public List<EnergyToday> GetSettlementHistoryTodayByHourPagination(long settlementId, long deviceCategoryId, int pageNumber, int itemsPerPage)
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
	                                            JOIN Devices d ON deu.DeviceId = d.Id
	                                            JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                                JOIN DeviceTypes dt ON dt.Id = dm.DeviceTypeId AND dt.CategoryId = @deviceCategoryId
                                                JOIN Users u ON d.UserId = u.Id AND u.SettlementId = @settlementId
                                            WHERE
	                                            deu.StartTime >= datetime('now', 'start of day') AND deu.StartTime <= datetime('now', 'localtime')
                                            GROUP BY
	                                            strftime('%Y-%m-%d %H:00:00', deu.StartTime)
                                        ) AS T
                                        WHERE RowNumber > @skipCount
                                        LIMIT @itemsPerPage";

                command.Parameters.Add(new SqliteParameter("@settlementId", settlementId));
                command.Parameters.Add(new SqliteParameter("@deviceCategoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@skipCount", skipCount));
                command.Parameters.Add(new SqliteParameter("@itemsPerPage", itemsPerPage));

                var energyUsages = new List<EnergyToday>();

                using (var reader = command.ExecuteReader())
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

                return energyUsages;
            }
        }
    }
}
