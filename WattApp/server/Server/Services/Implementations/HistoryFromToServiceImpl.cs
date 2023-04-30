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
            Console.WriteLine("/******************** from=" + fromDate + " --- " + toDate);

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT
	                                        SUM(CAST((strftime('%s', deu.EndTime) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
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
                        energyUsageResult = double.Parse(reader["EnergyUsageKwh"].ToString());
                }

                return energyUsageResult;
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
	                                    SUM(CAST((strftime('%s', deu.EndTime) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
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
                        energyUsageResult = double.Parse(reader["EnergyUsageKwh"].ToString());
                }

                return energyUsageResult;
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
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now')
								                                          THEN datetime('now')
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
	                                        SUM(CAST((strftime('%s', CASE WHEN deu.EndTime > datetime('now')
								                                          THEN datetime('now')
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
    }
}