﻿using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models.DropDowns.Location;
using System;
using System.Collections.Generic;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            DateTime FromDate;
            DateTime.TryParseExact(fromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
            DateTime ToDate;
            DateTime.TryParseExact(toDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);

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
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate AND deu.StartTime < datetime('now', 'localtime')";

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
            DateTime FromDate;
            DateTime.TryParseExact(fromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
            DateTime ToDate;
            DateTime.TryParseExact(toDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);

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
	                                    deu.StartTime >= @fromDate AND deu.StartTime <= @toDate AND deu.StartTime < datetime('now', 'localtime')";

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

        

        public List<DailyEnergyConsumptionPastMonth> GetCityHistoryByDayFromTo(string fromDate, string toDate, long deviceCategoryId, long cityId)
        {
            DateTime FromDate;
            DateTime.TryParseExact(fromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
            DateTime ToDate;
            DateTime.TryParseExact(toDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);

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
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate AND deu.StartTime < datetime('now', 'localtime')
                                        GROUP BY
	                                        DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@cityId", cityId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

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
                        energyUsages = FillDaysWithoutResults(FromDate, ToDate, energyUsages);
                    }
                    else
                        NoPaginationFillInWithZerosConsumptionProductionMonthByDay(FromDate, ToDate, energyUsages);
                }
                
                return energyUsages;
            }
        }

        

        public List<DailyEnergyConsumptionPastMonth> GetSettlementHistoryByDayFromTo(string fromDate, string toDate, long deviceCategoryId, long settlementId)
        {
            DateTime FromDate;
            DateTime.TryParseExact(fromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
            DateTime ToDate;
            DateTime.TryParseExact(toDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);

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
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate AND deu.StartTime < datetime('now', 'localtime')
                                        GROUP BY
	                                        DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@settlementId", settlementId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

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
                        energyUsages = FillDaysWithoutResults(FromDate, ToDate, energyUsages);
                    }
                    else
                        NoPaginationFillInWithZerosConsumptionProductionMonthByDay(FromDate, ToDate, energyUsages);
                }
                
                return energyUsages;
            }
        }

        public List<EnergyToday> GetCityHistoryByHourFromTo(string fromDate, string toDate, long deviceCategoryId, long cityId)
        {
            DateTime FromDate;
            DateTime.TryParseExact(fromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
            DateTime ToDate;
            DateTime.TryParseExact(toDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);

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
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate AND deu.StartTime < datetime('now', 'localtime')
                                        GROUP BY
	                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@cityId", cityId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

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
                        energyUsages = FillHoursWithoutResults(FromDate, ToDate, energyUsages);
                    }
                    else
                        NoPaginationFillInWithZerosConsumptionProductionDayByHour(FromDate, ToDate, energyUsages);
                }
                
                return energyUsages;
            }
        }

        public List<EnergyToday> GetSettlementHistoryByHourFromTo(string fromDate, string toDate, long deviceCategoryId, long settlementId)
        {
            DateTime FromDate;
            DateTime.TryParseExact(fromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
            DateTime ToDate;
            DateTime.TryParseExact(toDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);

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
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate AND deu.StartTime < datetime('now', 'localtime')
                                        GROUP BY
	                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@settlementId", settlementId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

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
                        energyUsages = FillHoursWithoutResults(FromDate, ToDate, energyUsages);
                    }
                    else
                        NoPaginationFillInWithZerosConsumptionProductionDayByHour(FromDate, ToDate, energyUsages);
                }
                
                return energyUsages;
            }
        }

        // PROSUMER
        public List<EnergyToday> GetProsumerHistoryByHourFromTo(string fromDate, string toDate, long userId, long categoryId)
        {
            DateTime FromDate;
            DateTime.TryParseExact(fromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
            DateTime ToDate;
            DateTime.TryParseExact(toDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);

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
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate AND deu.StartTime < datetime('now', 'localtime')
                                        GROUP BY
	                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@userId", userId));
                command.Parameters.Add(new SqliteParameter("@categoryId", categoryId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

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
                        energyUsages = FillHoursWithoutResults(FromDate, ToDate, energyUsages);
                    }
                    else
                        NoPaginationFillInWithZerosConsumptionProductionDayByHour(FromDate, ToDate, energyUsages);
                }
                
                return energyUsages;
            }
        }

        public List<DailyEnergyConsumptionPastMonth> GetProsumerHistoryByDayFromTo(string fromDate, string toDate, long userId, long categoryId)
        {
            DateTime FromDate;
            DateTime.TryParseExact(fromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
            DateTime ToDate;
            DateTime.TryParseExact(toDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);

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
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate AND deu.StartTime < datetime('now', 'localtime')
                                        GROUP BY
	                                        DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@userId", userId));
                command.Parameters.Add(new SqliteParameter("@categoryId", categoryId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

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
                        energyUsages = FillDaysWithoutResults(FromDate, ToDate, energyUsages);
                    }
                    else
                        NoPaginationFillInWithZerosConsumptionProductionMonthByDay(FromDate, ToDate, energyUsages);
                }
                
                return energyUsages;
            }
        }

        public double GetProsumerDoubleHistoryFromTo(string fromDate, string toDate, long userId, long categoryId)
        {
            if (!_context.Devices.Any(d => d.UserId == userId))
                return 0.0;

            DateTime FromDate;
            DateTime.TryParseExact(fromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
            DateTime ToDate;
            DateTime.TryParseExact(toDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);

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
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate AND deu.StartTime < datetime('now', 'localtime')";

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
            DateTime FromDate;
            DateTime.TryParseExact(fromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
            DateTime ToDate;
            DateTime.TryParseExact(toDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);

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
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate AND deu.StartTime < datetime('now', 'localtime')";

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

        

        public List<DailyEnergyConsumptionPastMonth> GetDeviceHistoryByDayFromTo(string fromDate, string toDate, long deviceId)
        {
            DateTime FromDate;
            DateTime.TryParseExact(fromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
            DateTime ToDate;
            DateTime.TryParseExact(toDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);

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
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate AND deu.StartTime < datetime('now', 'localtime')
                                        GROUP BY
	                                        DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@deviceId", deviceId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

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
                        energyUsages = FillDaysWithoutResults(FromDate, ToDate, energyUsages);
                    }
                    else
                        NoPaginationFillInWithZerosConsumptionProductionMonthByDay(FromDate, ToDate, energyUsages);
                }
                
                return energyUsages;
            }
        }

        public List<EnergyToday> GetDeviceHistoryByHourFromTo(string fromDate, string toDate, long deviceId)
        {
            DateTime FromDate;
            DateTime.TryParseExact(fromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out FromDate);
            DateTime ToDate;
            DateTime.TryParseExact(toDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ToDate);

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
	                                        deu.StartTime >= @fromDate AND deu.StartTime <= @toDate AND deu.StartTime < datetime('now', 'localtime')
                                        GROUP BY
	                                        strftime('%Y-%m-%d %H:00:00', deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@deviceId", deviceId));
                command.Parameters.Add(new SqliteParameter("@fromDate", FromDate));
                command.Parameters.Add(new SqliteParameter("@toDate", ToDate));

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
                        energyUsages = FillHoursWithoutResults(FromDate, ToDate, energyUsages);
                    }
                    else
                        NoPaginationFillInWithZerosConsumptionProductionDayByHour(FromDate, ToDate, energyUsages);
                }

                return energyUsages;
            }
        }

        // PAGINACIJA
        public ForByHourPaginationWithNumberOfRows GetDeviceHistoryByHourFromToPagination(string fromDate, string toDate, long deviceId, int pageNumber, int itemsPerPage)
        {
            var deviceListEnergyUsage = GetDeviceHistoryByHourFromTo(fromDate, toDate, deviceId);
            return InsertIntoForByHourPaginationWithNumberOfRowsModel(pageNumber, itemsPerPage, deviceListEnergyUsage);
        }

        public ForByHourPaginationWithNumberOfRows InsertIntoForByHourPaginationWithNumberOfRowsModel(int pageNumber, int itemsPerPage, List<EnergyToday> deviceListEnergyUsage)
        {
            int NumberOfRows = deviceListEnergyUsage.Count;
            int NumberOfPages = (int)Math.Ceiling((double)NumberOfRows / itemsPerPage);

            int fromIndex = (pageNumber - 1) * itemsPerPage;

            if (fromIndex + itemsPerPage > NumberOfRows)
                itemsPerPage = NumberOfRows - fromIndex;

            if (pageNumber < 0 || pageNumber > NumberOfPages)
                return null;

            var result = new ForByHourPaginationWithNumberOfRows
            {
                NumberOfRows = NumberOfRows,
                NumberOfPages = NumberOfPages,
                Data = deviceListEnergyUsage.GetRange(fromIndex, itemsPerPage)
            };

            return result;
        }

        public ForByDayPaginationWithNumberOfRowsAndPages InsertIntoForByDayPaginationWithNumberOfRowsModel(int pageNumber, int itemsPerPage, List<DailyEnergyConsumptionPastMonth> listEnergyUsage)
        {
            int NumberOfRows = listEnergyUsage.Count;
            int NumberOfPages = (int)Math.Ceiling((double)NumberOfRows / itemsPerPage);

            int fromIndex = (pageNumber - 1) * itemsPerPage;

            if (fromIndex + itemsPerPage > NumberOfRows)
                itemsPerPage = NumberOfRows - fromIndex;

            if (pageNumber < 0 || pageNumber > NumberOfPages)
                return null;

            var result = new ForByDayPaginationWithNumberOfRowsAndPages
            {
                NumberOfRows = NumberOfRows,
                NumberOfPages = NumberOfPages,
                Data = listEnergyUsage.GetRange(fromIndex, itemsPerPage),
            };

            return result;
        }

        public List<DailyEnergyConsumptionPastMonth> FillDaysWithoutResults(DateTime FromDate, DateTime ToDate, List<DailyEnergyConsumptionPastMonth> energyUsages)
        {
            /*if (ToDate > DateTime.Now)
                ToDate = DateTime.Now;*/

            int checker = 0;
            for (var date = FromDate; date < ToDate; date = date.AddDays(1))
            {
                checker = 0;
                foreach (var item in energyUsages)
                {
                    int monthNumber = GetMonthNumber(item.Month);
                    if (date.Day == item.Day && date.Month == monthNumber && date.Year == item.Year)
                    {
                        checker = 1;
                    }
                }
                if (checker == 0)
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
            }
            return energyUsages.OrderBy(eu => new DateTime(eu.Year, GetMonthNumber(eu.Month), eu.Day)).ToList();
        }

        private static int GetMonthNumber(string monthName)
        {
            return DateTime.ParseExact(monthName, "MMMM", CultureInfo.CurrentCulture).Month;
        }

        public List<EnergyToday> FillHoursWithoutResults(DateTime FromDate, DateTime ToDate, List<EnergyToday> energyUsages)
        {
            /*DateTime ToDate = toDate;
            if(toDate > DateTime.Now)
                ToDate = DateTime.Now;*/

            int checker = 0;
            for (var date = FromDate; date < ToDate; date = date.AddHours(1))
            {
                checker = 0;
                foreach (var item in energyUsages)
                {
                    int monthNumber = GetMonthNumber(item.Month);
                    if (date.Hour == item.Hour && date.Day == item.Day && date.Month == monthNumber && date.Year == item.Year)
                    {
                        checker = 1;
                    }
                }
                if (checker == 0)
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
            return energyUsages.OrderBy(eu => new DateTime(eu.Year, GetMonthNumber(eu.Month), eu.Day, eu.Hour, 0, 0)).ToList();
        }

        public ForByHourPaginationWithNumberOfRows GetUserHistoryByHourFromToPagination(string fromDate, string toDate, long userId, long deviceCategoryId, int pageNumber, int itemsPerPage)
        {
            var userListEnergyUsage = GetProsumerHistoryByHourFromTo(fromDate, toDate, userId, deviceCategoryId);
            return InsertIntoForByHourPaginationWithNumberOfRowsModel(pageNumber, itemsPerPage, userListEnergyUsage);
        }

        public ForByHourPaginationWithNumberOfRows GetSettlementHistoryByHourFromToPagination(string fromDate, string toDate, long settlementId, long deviceCategoryId, int pageNumber, int itemsPerPage)
        {
            var settlementListEnergyUsage = GetSettlementHistoryByHourFromTo(fromDate, toDate, deviceCategoryId, settlementId);
            return InsertIntoForByHourPaginationWithNumberOfRowsModel(pageNumber, itemsPerPage, settlementListEnergyUsage);
        }

        public ForByHourPaginationWithNumberOfRows GetCityHistoryByHourFromToPagination(string fromDate, string toDate, long cityId, long deviceCategoryId, int pageNumber, int itemsPerPage)
        {
            var cityListEnergyUsage = GetCityHistoryByHourFromTo(fromDate, toDate, deviceCategoryId, cityId);
            return InsertIntoForByHourPaginationWithNumberOfRowsModel(pageNumber, itemsPerPage, cityListEnergyUsage);
        }

        public ForByDayPaginationWithNumberOfRowsAndPages GetCityHistoryByDayFromToPagination(string fromDate, string toDate, long cityId, long deviceCategoryId, int pageNumber, int itemsPerPage)
        {
            var cityListEnergyUsage = GetCityHistoryByDayFromTo(fromDate, toDate, deviceCategoryId, cityId);
            return InsertIntoForByDayPaginationWithNumberOfRowsModel(pageNumber, itemsPerPage, cityListEnergyUsage);
        }

        public ForByDayPaginationWithNumberOfRowsAndPages GetSettlementHistoryByDayFromToPagination(string fromDate, string toDate, long settlementId, long deviceCategoryId, int pageNumber, int itemsPerPage)
        {
            var settlementListEnergyUsage = GetSettlementHistoryByDayFromTo(fromDate, toDate, deviceCategoryId, settlementId);
            return InsertIntoForByDayPaginationWithNumberOfRowsModel(pageNumber, itemsPerPage, settlementListEnergyUsage);
        }

        public ForByDayPaginationWithNumberOfRowsAndPages GetUserHistoryByDayFromToPagination(string fromDate, string toDate, long userId, long deviceCategoryId, int pageNumber, int itemsPerPage)
        {
            var userListEnergyUsage = GetProsumerHistoryByDayFromTo(fromDate, toDate, userId, deviceCategoryId);
            return InsertIntoForByDayPaginationWithNumberOfRowsModel(pageNumber, itemsPerPage, userListEnergyUsage);
        }

        public ForByDayPaginationWithNumberOfRowsAndPages GetDeviceHistoryByDayFromToPagination(string fromDate, string toDate, long deviceId, int pageNumber, int itemsPerPage)
        {
            var deviceListEnergyUsage = GetDeviceHistoryByDayFromTo(fromDate, toDate, deviceId);
            return InsertIntoForByDayPaginationWithNumberOfRowsModel(pageNumber, itemsPerPage, deviceListEnergyUsage);
        }

        public void NoPaginationFillInWithZerosConsumptionProductionDayByHour(DateTime FromDate, DateTime ToDate, List<EnergyToday> energyUsages)
        {
            /*if(ToDate > DateTime.Now)
                ToDate = DateTime.Now;*/

            for (var date = FromDate; date < ToDate; date = date.AddHours(1))
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

        public void NoPaginationFillInWithZerosConsumptionProductionMonthByDay(DateTime FromDate, DateTime ToDate, List<DailyEnergyConsumptionPastMonth> energyUsages)
        {
            /*if (ToDate > DateTime.Now)
                ToDate = DateTime.Now;*/

            for (var date = FromDate; date < ToDate; date = date.AddDays(1))
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
        }
    }
}