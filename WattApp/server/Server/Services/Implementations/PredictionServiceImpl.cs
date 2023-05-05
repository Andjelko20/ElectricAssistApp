using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;

namespace Server.Services.Implementations
{
    public class PredictionServiceImpl : IPredictionService
    {
        private readonly SqliteDbContext _context;
        public PredictionServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public List<DailyEnergyConsumptionPastMonth> ConsumptionPredictionForTheNextWeek(long deviceId)
        {
            var Device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            var StartDate = DateTime.Now.Date.AddDays(1);
            var EndDate = StartDate.AddDays(7).AddSeconds(-1);

            var UsageList = _context.DeviceEnergyUsages
                            .Where(u => u.DeviceId == deviceId && u.StartTime >= StartDate && u.EndTime <= EndDate)
                            .ToList();

            var Results = new List<DailyEnergyConsumptionPastMonth>();

            for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
            {
                var UsageForDate = UsageList.Where(u => u.StartTime.Date == date.Date).ToList(); // za taj dan

                double EnergyUsage = 0.0;
                foreach (var usage in UsageForDate) // za taj dan
                {
                    var DeviceModel = _context.DeviceModels.FirstOrDefault(dm => dm.Id == Device.DeviceModelId);
                    var EnergyKwh = DeviceModel.EnergyKwh;
                    EnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * EnergyKwh;// Device.EnergyInKwh; // za svaki period kada je radio izracunaj koliko je trosio
                }

                Results.Add(new DailyEnergyConsumptionPastMonth // klasa moze i za week
                {
                    Day = date.Day,
                    Month = date.ToString("MMMM"),
                    Year = date.Year,
                    EnergyUsageResult = Math.Round(EnergyUsage, 2)
                });
            }

            return Results;
        }

        public List<DailyEnergyConsumptionPastMonth> UserPredictionForTheNextWeek(long userId, long deviceCategoryId)
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
                                        WHERE deu.StartTime >= date('now', '+1 days') 
                                            AND (deu.EndTime <= date('now', '+8 days') OR deu.EndTime IS NULL)
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
                        var startDate = DateTime.Now.Date.AddDays(1);
                        var endDate = startDate.AddDays(7);

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

        public List<DailyEnergyConsumptionPastMonth> CityPredictionForTheNextWeek(long cityId, long deviceCategoryId)
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
							            JOIN Users u ON d.UserId=u.Id
                                        JOIN Settlements s ON s.Id = u.SettlementId AND s.CityId = @cityId
                                        WHERE deu.StartTime >= date('now', '+1 days') 
                                            AND (deu.EndTime <= date('now', '+8 days') OR deu.EndTime IS NULL)
                                        GROUP BY DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@deviceCategoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@cityId", cityId));

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
                        var startDate = DateTime.Now.Date.AddDays(1);
                        var endDate = startDate.AddDays(7);

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

        public List<DailyEnergyConsumptionPastMonth> SettlementPredictionForTheNextWeek(long settlementId, long deviceCategoryId)
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
							            JOIN Users u ON d.UserId=u.Id AND u.SettlementId = @settlementId
                                        WHERE deu.StartTime >= date('now', '+1 days') 
                                            AND (deu.EndTime <= date('now', '+8 days') OR deu.EndTime IS NULL)
                                        GROUP BY DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@deviceCategoryId", deviceCategoryId));
                command.Parameters.Add(new SqliteParameter("@settlementId", settlementId));

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
                        var startDate = DateTime.Now.Date.AddDays(1);
                        var endDate = startDate.AddDays(7);

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

        // PREDICTION FOR PREVIUOS 7 DAYS
        public List<DailyEnergyConsumptionPastMonth> CityPredictionForThePastWeek(long cityId, long deviceCategoryId)
        {
            Random random = new Random();
            double minValue = -1000.0;
            double maxValue = 1000.0;
            if (deviceCategoryId == 1)
            {
                minValue = 0.0;
                maxValue = 500.0;
            }

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT DATE(deu.StartTime) AS YYMMDD,
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
                        DateTime date = DateTime.ParseExact(reader["YYMMDD"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var day = date.Day;
                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

                        double randomNumber = random.NextDouble() * (maxValue - minValue) + minValue;

                        var dailyEnergyUsage = new DailyEnergyConsumptionPastMonth
                        {
                            Day = day,
                            Month = month,
                            Year = year,
                            EnergyUsageResult = Math.Round(energyUsage + randomNumber, 2)
                        };

                        energyUsages.Add(dailyEnergyUsage);
                    }
                }

                return energyUsages;
            }
        }

        public List<DailyEnergyConsumptionPastMonth> SettlementPredictionForThePastWeek(long settlementId, long deviceCategoryId)
        {
            Random random = new Random();
            double minValue = -50.0;
            double maxValue = 500.0;

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT DATE(deu.StartTime) AS YYMMDD,
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
                        DateTime date = DateTime.ParseExact(reader["YYMMDD"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        var day = date.Day;
                        var month = date.ToString("MMMM");
                        var year = date.Year;
                        var energyUsage = double.Parse(reader["EnergyUsageKwh"].ToString());

                        double randomNumber = random.NextDouble() * (maxValue - minValue) + minValue;

                        var dailyEnergyUsage = new DailyEnergyConsumptionPastMonth
                        {
                            Day = day,
                            Month = month,
                            Year = year,
                            EnergyUsageResult = Math.Round(energyUsage + randomNumber, 2)
                        };

                        energyUsages.Add(dailyEnergyUsage);
                    }
                }

                return energyUsages;
            }
        }

        public List<DailyEnergyConsumptionPastMonth> ProsumerPredictionForThePastWeek(long userId, long deviceCategoryId)
        {
            Random random = new Random();
            double minValue = -5.0;
            double maxValue = 50.0;

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT DATE(deu.StartTime) AS YYMMDD,
                                               SUM(CAST((strftime('%s', deu.EndTime) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu 
                                        JOIN Devices d ON deu.DeviceId = d.Id AND d.UserId = @userId
                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                        JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id AND dt.CategoryId = @categoryId
                                        WHERE deu.StartTime >= date('now', '-7 day') 
                                            AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)
                                        GROUP BY DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@categoryId", deviceCategoryId));
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

                            double randomNumber = random.NextDouble() * (maxValue - minValue) + minValue;

                            var dailyEnergyUsage = new DailyEnergyConsumptionPastMonth
                            {
                                Day = day,
                                Month = month,
                                Year = year,
                                EnergyUsageResult = Math.Round(energyUsage + randomNumber, 2)
                            };

                            energyUsages.Add(dailyEnergyUsage);
                        }
                    }
                    else
                    {
                        var startDate = DateTime.Now.Date.AddDays(-7);
                        var endDate = DateTime.Now.Date;

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

        public List<DailyEnergyConsumptionPastMonth> DevicePredictionForThePastWeek(long deviceId)
        {
            Random random = new Random();
            double minValue = 0.0;
            double maxValue = 3.0;

            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT DATE(deu.StartTime) AS YYMMDD,
                                               SUM(CAST((strftime('%s', deu.EndTime) - strftime('%s', deu.StartTime)) / 3600.0 AS REAL) * dm.EnergyKwh) AS EnergyUsageKwh
                                        FROM DeviceEnergyUsages deu 
                                        JOIN Devices d ON deu.DeviceId = d.Id AND d.Id = @deviceId
                                        JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
                                        WHERE deu.StartTime >= date('now', '-7 day') 
                                            AND (deu.EndTime <= date('now') OR deu.EndTime IS NULL)
                                        GROUP BY DATE(deu.StartTime)";

                command.Parameters.Add(new SqliteParameter("@deviceId", deviceId));

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

                            double randomNumber = random.NextDouble() * (maxValue - minValue) + minValue;

                            var dailyEnergyUsage = new DailyEnergyConsumptionPastMonth
                            {
                                Day = day,
                                Month = month,
                                Year = year,
                                EnergyUsageResult = Math.Round(energyUsage + randomNumber, 2)
                            };

                            energyUsages.Add(dailyEnergyUsage);
                        }
                    }
                    else
                    {
                        var startDate = DateTime.Now.Date.AddDays(-7);
                        var endDate = DateTime.Now.Date;

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
    }
}
