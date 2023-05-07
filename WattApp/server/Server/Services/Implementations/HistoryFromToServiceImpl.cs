using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using System.Globalization;

namespace Server.Services.Implementations
{
    public class HistoryFromToServiceImpl : IHistoryFromToService
    {
        private readonly SqliteDbContext _context;
        public HistoryFromToServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public double GetCityDoubleHistoryFromTo(string fromDate, string toDate, long deviceCategoryId, long cityId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @deviceCategoryId
	                                        JOIN Users u ON d.UserId = u.Id
	                                        JOIN Settlements s ON s.Id = u.SettlementId AND s.CityId = @cityId
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate";

                command.Parameters.Add(new SqliteParameter("@deviceCategoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@cityId", cityId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

                double energyUsageResult = 0;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                            energyUsageResult = energyUsage;
                }

                return Math.Round(energyUsageResult, 2);
            }
        }

        public double GetSettlementDoubleHistoryFromTo(string fromDate, string toDate, long deviceCategoryId, long settlementId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                    SELECT
	                                    SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                     END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                    FROM
	                                    DeviceEnergyUsages deu
	                                    JOIN Devices d ON deu.DeviceId = d.Id
	                                    JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                    JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @deviceCategoryId
	                                    JOIN Users u ON d.UserId = u.Id AND u.SettlementId = @settlementId
                                    WHERE
	                                    deu.StartTime >= @fromDate AND deu.StartTime <= @toDate";

                command.Parameters.Add(new SqliteParameter("@deviceCategoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@settlementId", settlementId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

                double energyUsageResult = 0;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                            energyUsageResult = energyUsage;
                }

                return Math.Round(energyUsageResult, 2);
            }
        }

        public List<MonthlyEnergyConsumptionLastYear> GetCityHistoryByMonthFromTo(string fromDate, string toDate, long deviceCategoryId, long cityId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        strftime('%Y-%m', deu.StartTime) AS Datum,
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
	                                        JOIN Users u ON d.UserId = u.Id
	                                        JOIN Settlements s ON s.Id = u.SettlementId AND s.CityId = @cityId
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate
                                        GROUP BY
	                                         strftime('%Y-%m', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@cityId", cityId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

                var energyUsages = new List<MonthlyEnergyConsumptionLastYear>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader["Datum"].ToString(), "yyyy-MM", CultureInfo.InvariantCulture);

                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        double energyUsageResult = 0.0;
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                        {
                            energyUsageResult = energyUsage;
                        }

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

        public List<DailyEnergyConsumptionPastMonth> GetCityHistoryByDayFromTo(string fromDate, string toDate, long deviceCategoryId, long cityId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        DATE(deu.StartTime) AS Datum,
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
	                                        JOIN Users u ON d.UserId = u.Id
	                                        JOIN Settlements s ON s.Id = u.SettlementId AND s.CityId = @cityId
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate
                                        GROUP BY
	                                        DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@cityId", cityId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

                var energyUsages = new List<DailyEnergyConsumptionPastMonth>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader["Datum"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var day = date.Day;
                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        double energyUsageResult = 0.0;
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                        {
                            energyUsageResult = energyUsage;
                        }

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

        public List<MonthlyEnergyConsumptionLastYear> GetSettlementHistoryByMonthFromTo(string fromDate, string toDate, long deviceCategoryId, long settlementId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        strftime('%Y-%m', deu.StartTime) AS Datum,
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
	                                        JOIN Users u ON d.UserId = u.Id AND u.SettlementId = @settlementId
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate
                                        GROUP BY
	                                        strftime('%Y-%m', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@settlementId", settlementId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

                var energyUsages = new List<MonthlyEnergyConsumptionLastYear>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader["Datum"].ToString(), "yyyy-MM", CultureInfo.InvariantCulture);

                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        double energyUsageResult = 0.0;
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                        {
                            energyUsageResult = energyUsage;
                        }

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

        public List<DailyEnergyConsumptionPastMonth> GetSettlementHistoryByDayFromTo(string fromDate, string toDate, long deviceCategoryId, long settlementId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        DATE(deu.StartTime) AS Datum,
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
	                                        JOIN Users u ON d.UserId = u.Id AND u.SettlementId = @settlementId
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate
                                        GROUP BY
	                                        DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@settlementId", settlementId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

                var energyUsages = new List<DailyEnergyConsumptionPastMonth>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader["Datum"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var day = date.Day;
                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        double energyUsageResult = 0.0;
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                        {
                            energyUsageResult = energyUsage;
                        }

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

        public List<EnergyToday> GetCityHistoryByHourFromTo(string fromDate, string toDate, long deviceCategoryId, long cityId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime) AS Datum,
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
	                                        JOIN Users u ON d.UserId = u.Id
                                            JOIN Settlements s ON u.SettlementId = s.Id AND s.cityId = @cityId 
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate
                                        GROUP BY
	                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@cityId", cityId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

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
                        double energyUsageResult = 0.0;
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                        {
                            energyUsageResult = energyUsage;
                        }

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

        public List<EnergyToday> GetSettlementHistoryByHourFromTo(string fromDate, string toDate, long deviceCategoryId, long settlementId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime) AS Datum,
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
	                                        JOIN Users u ON d.UserId = u.Id AND u.SettlementId = @settlementId
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate
                                        GROUP BY
	                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@settlementId", settlementId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

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
                        double energyUsageResult = 0.0;
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                        {
                            energyUsageResult = energyUsage;
                        }

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

        // PROSUMER
        public List<EnergyToday> GetProsumerHistoryByHourFromTo(string fromDate, string toDate, long userId, long categoryId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime) AS Datum,
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id AND d.UserId = @userId
                                            JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate
                                        GROUP BY
	                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@userId", userId));
                command.Parameters.Add(new SqliteParameter("@categoryId", categoryId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

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
                        double energyUsageResult = 0.0;
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                        {
                            energyUsageResult = energyUsage;
                        }

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

        public List<MonthlyEnergyConsumptionLastYear> GetProsumerHistoryByMonthFromTo(string fromDate, string toDate, long userId, long categoryId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        strftime('%Y-%m', deu.StartTime) AS Datum,
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id AND d.UserId = @userId
                                            JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate
                                        GROUP BY
	                                        strftime('%Y-%m', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@userId", userId));
                command.Parameters.Add(new SqliteParameter("@categoryId", categoryId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

                var energyUsages = new List<MonthlyEnergyConsumptionLastYear>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader["Datum"].ToString(), "yyyy-MM", CultureInfo.InvariantCulture);

                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        double energyUsageResult = 0.0;
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                        {
                            energyUsageResult = energyUsage;
                        }

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

        public List<DailyEnergyConsumptionPastMonth> GetProsumerHistoryByDayFromTo(string fromDate, string toDate, long userId, long categoryId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        DATE(deu.StartTime) AS Datum,
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id AND d.UserId = @userId
                                            JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate
                                        GROUP BY
	                                        DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@userId", userId));
                command.Parameters.Add(new SqliteParameter("@categoryId", categoryId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

                var energyUsages = new List<DailyEnergyConsumptionPastMonth>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader["Datum"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var day = date.Day;
                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        double energyUsageResult = 0.0;
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                        {
                            energyUsageResult = energyUsage;
                        }

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

        public double GetProsumerDoubleHistoryFromTo(string fromDate, string toDate, long userId, long categoryId)
        {
            if(!_context.Devices.Any(d => d.UserId == userId))
                return 0.0;

            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id AND d.UserId = @userId
                                            JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
	                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate";

                command.Parameters.Add(new SqliteParameter("@userId", userId));
                command.Parameters.Add(new SqliteParameter("@categoryId", categoryId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

                double energyUsageResult = 0;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                        {
                            energyUsageResult = energyUsage;
                        }
                    }
                }

                return Math.Round(energyUsageResult, 2);
            }
        }

        public double GetDeviceDoubleHistoryFromTo(string fromDate, string toDate, long deviceId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id AND deu.DeviceId = @deviceId
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate";

                command.Parameters.Add(new SqliteParameter("@deviceId", deviceId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

                double energyUsageResult = 0;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                            energyUsageResult = energyUsage;
                }

                return Math.Round(energyUsageResult, 2);
            }
        }

        public List<MonthlyEnergyConsumptionLastYear> GetDeviceHistoryByMonthFromTo(string fromDate, string toDate, long deviceId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        strftime('%Y-%m', deu.StartTime) AS Datum,
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id AND deu.DeviceId = @deviceId
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate
                                        GROUP BY
	                                        strftime('%Y-%m', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@deviceId", deviceId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

                var energyUsages = new List<MonthlyEnergyConsumptionLastYear>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader["Datum"].ToString(), "yyyy-MM", CultureInfo.InvariantCulture);

                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        double energyUsageResult = 0.0;
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                        {
                            energyUsageResult = energyUsage;
                        }

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

        public List<DailyEnergyConsumptionPastMonth> GetDeviceHistoryByDayFromTo(string fromDate, string toDate, long deviceId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        DATE(deu.StartTime) AS Datum,
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id AND deu.DeviceId = @deviceId
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate
                                        GROUP BY
	                                        DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@deviceId", deviceId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

                var energyUsages = new List<DailyEnergyConsumptionPastMonth>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.ParseExact(reader["Datum"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var day = date.Day;
                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        double energyUsageResult = 0.0;
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                        {
                            energyUsageResult = energyUsage;
                        }

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

        public List<EnergyToday> GetDeviceHistoryByHourFromTo(string fromDate, string toDate, long deviceId)
        {
            DateTime FromDate = DateTime.Parse(fromDate);
            DateTime ToDate = DateTime.Parse(toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime) AS Datum,
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now', '+2 hours')
								                                          THEN datetime('now', '+2 hours')
                                                                          WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
								                                          ELSE deu.EndTime
							                                         END) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM
	                                        DeviceEnergyUsages deu
	                                        JOIN Devices d ON deu.DeviceId = d.Id AND deu.DeviceId = @deviceId
	                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                        WHERE
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate
                                        GROUP BY
	                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@deviceId", deviceId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

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
                        double energyUsageResult = 0.0;
                        if (double.TryParse(reader["EnergyUsageKwh"]?.ToString(), out double energyUsage))
                        {
                            energyUsageResult = energyUsage;
                        }

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