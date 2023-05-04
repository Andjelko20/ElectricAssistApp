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

        public double GetUsageHistoryForDeviceFromCurrentYear(long deviceId)
        {
            DateTime startOfTheYear = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
            DateTime theTime = DateTime.Now;
            double consumption = 0.0;

            var deviceModelEnergyKwh = _context.Devices
                .Where(d => d.Id == deviceId)
                .Select(d => d.DeviceModel.EnergyKwh)
                .FirstOrDefault();

            var energyUsages = _context.DeviceEnergyUsages
                .Where(du => du.DeviceId == deviceId && du.StartTime >= startOfTheYear && (du.EndTime <= theTime || du.EndTime == null))
                .ToList();

            foreach (var energyUsage in energyUsages)
            {
                var hours = Math.Abs((energyUsage.EndTime - energyUsage.StartTime).TotalHours);
                consumption += (double)(deviceModelEnergyKwh * hours);
            }

            return Math.Round(consumption, 2);
        }

        public double GetUsageHistoryForDeviceFromCurrentMonth(long deviceId)
        {
            DateTime startOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            DateTime TheTime = DateTime.Now;

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= startOfTheMonth && (u.EndTime <= TheTime || u.EndTime == null))
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
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
                Hours = Math.Abs((item.EndTime - item.StartTime).TotalHours);
                Consumption += (double)(EnergyInKwh * Hours);
            }

            return Math.Round(Consumption, 2);
        }

        public double GetUsageHistoryForDeviceFromCurrentDay(long deviceId)
        {
            DateTime startOfTheDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime TheTime = DateTime.Now;

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= startOfTheDay && u.EndTime <= TheTime)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }

        public List<MonthlyEnergyConsumptionLastYear> GetUsageHistoryForDeviceFromCurrentYearByMonth(long deviceId)
        {
            var Device = _context.Devices.Find(deviceId);
            var DeviceModel = _context.DeviceModels.Find(Device.DeviceModelId);
            float EnergyInKwh = DeviceModel.EnergyKwh;

            DateTime TheTime = DateTime.Now;
            DateTime datum = DateTime.Now;
            int redniBrojMeseca = datum.Month;

            var Results = new List<MonthlyEnergyConsumptionLastYear>();
            for (int i = 0; i < redniBrojMeseca; i++)
            {
                var StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-i).Date;
                var EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-i + 1).AddDays(-1).Date.AddDays(1).AddSeconds(-1);

                var UsageList = _context.DeviceEnergyUsages
                                .Where(u => u.DeviceId == deviceId && u.StartTime.Month == redniBrojMeseca && u.EndTime <= EndDate)
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
                        UsageInHours = (item.EndTime - item.StartTime).TotalHours;
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

        public List<DailyEnergyConsumptionPastMonth> GetUsageHistoryForDeviceFromCurrentMonthByDay(long deviceId)
        {
            var Device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            var DeviceModel = _context.DeviceModels.FirstOrDefault(dm => dm.Id == Device.DeviceModelId);
            float EnergyInKwh = DeviceModel.EnergyKwh;
            DateTime StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime EndDate = StartDate.AddMonths(1).AddDays(-1).Date.AddDays(1).AddSeconds(-1);

            var UsageList = _context.DeviceEnergyUsages
                            .Where(u => u.DeviceId == deviceId && u.StartTime >= StartDate && u.EndTime <= EndDate)
                            .ToList();

            var Results = new List<DailyEnergyConsumptionPastMonth>();

            for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
            {
                var UsageForDate = UsageList.Where(u => u.StartTime.Date == date.Date).ToList();

                double EnergyUsage = 0.0;
                foreach (var usage in UsageForDate)
                    EnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * EnergyInKwh;

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

        public List<EnergyToday> GetUsageHistoryForProsumerFromCurrentDayByHour(long userId, long deviceCategoryId)
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

        public List<DailyEnergyConsumptionPastMonth> GetUsageHistoryForProsumerFromCurrentMonthByDay(long userId, long deviceCategoryId)
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
                                        WHERE deu.StartTime >= date('now', 'start of month') 
                                            AND (deu.EndTime <= strftime('%Y-%m-%d %H:%M:%S', 'now') OR deu.EndTime IS NULL)
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

        public List<MonthlyEnergyConsumptionLastYear> GetUsageHistoryForProsumerFromCurrentYearByMonth(long userId, long deviceCategoryId)
        {
            var userDevices = _context.Devices
                                .Include(d => d.DeviceModel)
                                .ThenInclude(dm => dm.DeviceType)
                                .ThenInclude(dt => dt.DeviceCategory)
                                .Where(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId).ToList();

            var endDate = DateTime.Now;
            var startDate = new DateTime(endDate.Year, 1, 1);
            int redniBrojMeseca = endDate.Month;

            var monthlyUsage = new List<MonthlyEnergyConsumptionLastYear>();

            for (var i = 0; i < redniBrojMeseca; i++)
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
                        monthlyEnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * EnergyInKwh;
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
                    totalEnergyConsumption += (usage.EndTime - usage.StartTime).TotalHours * EnergyInKwh;
                }
            }

            return Math.Round(totalEnergyConsumption, 2);
        }
    }
}
