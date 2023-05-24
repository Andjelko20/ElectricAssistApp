using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Polly;
using Server.Data;
using Server.DTOs;
using Server.Models;
using System.Globalization;
using System.Linq;

namespace Server.Services.Implementations
{
    public class ProsumerServiceImpl : IProsumerService
    {
        private readonly SqliteDbContext _context;
        public ProsumerServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        /*GetTotalConsumptionInTheMoment
        {
            double TotalEnergyUsage = -100.0;
            var Devices = _context.Devices.Where(d => _context.DeviceModels
                                                      .Where(dm => _context.DeviceTypes
                                                                   .Where(dt => dt.CategoryId == 1)
                                                                   .Select(dt => dt.Id)
                                                                   .Contains(dm.DeviceTypeId))
                                                      .Select(dm => dm.Id)
                                                      .Contains(d.DeviceModelId))
                                           .ToList();

            foreach (var Device in Devices)
            {
                var DeviceUsages = _context.DeviceEnergyUsages
                    .Where(u => u.DeviceId == Device.Id && u.StartTime <= DateTime.Now.AddHours(-1) && u.EndTime <= DateTime.Now)
                    .ToList();

                foreach (var usage in DeviceUsages)
                {
                    TotalEnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * Device.EnergyInKwh;
                }
            }

            return TotalEnergyUsage;
        }*/

        public double GetTotalConsumptionInTheMoment(long deviceCategoryId)
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
                .Where(d => deviceModelIds.Contains(d.DeviceModelId) && d.TurnOn == true)
                .Select(d => d.Id)
                .ToList();

            // pronalazimo uredjaje te kategorije u tabeli DeviceEnergyUsages i to ako trenutno rade
            var usages = _context.DeviceEnergyUsages
                .Where(u => devices.Contains(u.DeviceId) && u.StartTime <= now && (u.EndTime >= now || u.EndTime == null))
                .ToList();

            foreach (var usage in usages)
            {
                var usageStart = usage.StartTime;
                /*if (usageStart < now)
                {
                    usageStart = now;
                }*/

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
        }

        public double GetTotalConsumptionInTheMomentForSettlement(long deviceCategoryId, long settlementId)
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
                .Where(u => devices.Contains(u.DeviceId) && u.StartTime <= now && (u.EndTime >= now || u.EndTime == null) && _context.Devices.Any(d => d.Id == u.DeviceId && _context.Users.Any(u => u.Id == d.UserId && u.SettlementId == settlementId)))
                .ToList();

            foreach (var usage in usages)
            {
                var usageStart = usage.StartTime;
                /*if (usageStart < now)
                {
                    usageStart = now;
                }*/

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
        }

        public double GetTotalConsumptionInTheMomentForCity(long deviceCategoryId, long cityId)
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
                .Where(u => devices.Contains(u.DeviceId) && u.StartTime <= now && (u.EndTime >= now || u.EndTime == null) && _context.Devices.Any(d => d.Id == u.DeviceId && _context.Users.Any(u => u.Id == d.UserId && _context.Settlements.Any(s => s.Id == u.SettlementId && s.CityId == cityId && _context.Cities.Any(c => c.Id == s.CityId))))).ToList();

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
        }

        public double GetTotalConsumptionInTheMomentForOneProsumer(long deviceCategoryId, long userId)
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

            // pronalazimo sve uredjaje koji koriste te modele uredjaja i pripadaju datom korisniku
            var devices = _context.Devices
                .Where(d => deviceModelIds.Contains(d.DeviceModelId) && d.UserId == userId && d.TurnOn == true)
                .Select(d => d.Id)
                .ToList();

            // pronalazimo uredjaje te kategorije u tabeli DeviceEnergyUsages i to ako trenutno rade
            var usages = _context.DeviceEnergyUsages
                .Where(u => devices.Contains(u.DeviceId) && u.StartTime <= now && (u.EndTime >= now || u.EndTime == null) && _context.Devices.Any(d => d.Id == u.DeviceId && d.UserId == userId)).ToList();

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
        }

        public double GetNumberOfProsumersFromSettlement(long settlementId)
        {
            var totalPopulation = _context.Users
                                    .Where(u => u.SettlementId == settlementId)
                                    .Count();

            return totalPopulation;
        }

        public double GetNumberOfProsumersFromCity(long cityId)
        {
            var totalPopulation = _context.Users
                                        .Where(u => u.Settlement.City.Id == cityId)
                                        .Count();

            return totalPopulation;
        }

        public double GetAverageConsumptionInTheMomentForSettlement(long settlementId, double totalEnergyUsage)
        {
            var totalPopulation = GetNumberOfProsumersFromSettlement(settlementId);
            if (totalPopulation == 0)
                return 0;
            return Math.Round(totalEnergyUsage / totalPopulation, 2);
        }

        public double GetAverageConsumptionInTheMomentForCity(long settlementId, double totalEnergyUsage)
        {
            var totalPopulation = GetNumberOfProsumersFromCity(settlementId);
            if (totalPopulation == 0)
                return 0;
            return Math.Round(totalEnergyUsage / totalPopulation, 2);
        }

        public double GetAverageConsumptionProductionInTheMomentForAllProsumers(double totalEnergyUsage)
        {
            var totalPopulation = _context.Users.Count();

            if (totalPopulation == 0)
                return 0;

            return Math.Round(totalEnergyUsage / totalPopulation, 2);
        }

        public double GetTotalNumberOfDevicesInTheCity(long deviceCategoryId, long cityId)
        {
            // dodaj za grad kad se promene modeli
            double NumberOfDevices = 0.0;
            var Devices = _context.Devices.Where(d => _context.DeviceModels
                                                      .Where(dm => _context.DeviceTypes
                                                                   .Where(dt => dt.CategoryId == deviceCategoryId)
                                                                   .Select(dt => dt.Id)
                                                                   .Contains(dm.DeviceTypeId))
                                                      .Select(dm => dm.Id)
                                                      .Contains(d.DeviceModelId))
                                           .ToList();
            NumberOfDevices = Devices.Count;

            return NumberOfDevices;
        }

        public double GetTotalNumberOfDevicesInTheSettlement(long deviceCategoryId, long cityId, long settlementId)
        {
            // dodaj za naselje kad se promene modeli
            double NumberOfDevices = 0.0;
            var Devices = _context.Devices.Where(d => _context.DeviceModels
                                                      .Where(dm => _context.DeviceTypes
                                                                   .Where(dt => dt.CategoryId == deviceCategoryId)
                                                                   .Select(dt => dt.Id)
                                                                   .Contains(dm.DeviceTypeId))
                                                      .Select(dm => dm.Id)
                                                      .Contains(d.DeviceModelId))
                                           .ToList();
            NumberOfDevices = Devices.Count;

            return NumberOfDevices;
        }

        public List<EnergyToday> CalculateEnergyUsageForToday(long deviceId)
        {
            var device = _context.Devices
                .Include(d => d.DeviceModel) // da bih izvukao EnergyKwh odatle
                .FirstOrDefault(d => d.Id == deviceId);

            if (device == null)
                return null;

            var energyKwh = device.DeviceModel.EnergyKwh;

            var Result = new List<EnergyToday>();

            // racunamo potrosnju trazenog uredjaja od 00:00h do ovog trenutka, danasnjeg dana
            var deviceEnergyUsages = _context.DeviceEnergyUsages
                .Where(usage => usage.DeviceId == deviceId && usage.StartTime.Date == DateTime.Today)
                .ToList();

            // prolazimo kroz sve sate danasnjeg dana do ovog trenutka i racunamo potrosnju za svaki sat
            for (int hour = 0; hour <= DateTime.Now.Hour; hour++)
            {
                var startTime = DateTime.Today.AddHours(hour);
                var endTime = DateTime.Today.AddHours(hour + 1);
                var energyUsageResult = 0.0;
                foreach (var usage in deviceEnergyUsages)
                {
                    DateTime overlapStart;
                    if (usage.StartTime < startTime)
                    {
                        overlapStart = startTime;
                    }
                    else
                    {
                        overlapStart = usage.StartTime;
                    }

                    DateTime overlapEnd;
                    if (usage.EndTime == null || usage.EndTime > endTime)
                    {
                        overlapEnd = endTime;
                    }
                    else
                    {
                        if (usage.EndTime > DateTime.Now)
                            overlapEnd = DateTime.Now;
                        else
                            overlapEnd = (DateTime)usage.EndTime;
                    }

                    if (overlapStart < overlapEnd)
                    {
                        var durationInHours = (overlapEnd - overlapStart).TotalHours;
                        energyUsageResult += durationInHours * energyKwh;
                    }
                }

                Result.Add(new EnergyToday
                {
                    EnergyUsageResult = Math.Round(energyUsageResult, 2),
                    Hour = startTime.Hour,
                    Day = DateTime.Now.Day,
                    Month = DateTime.Now.ToString("MMMM"),
                    Year = DateTime.Now.Year
                });
            }

            return Result;
        }

        public int GetNumberOfDevicesOfOneProsumer(long userId)
        {
            var connection = _context.Database.GetDbConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(*) FROM Devices WHERE UserId = {userId}";

            int result = Convert.ToInt32(command.ExecuteScalar()); // ne dozvoljava int jer je 64b

            connection.Close();

            return result;
        }

        public double GetUserEnergyConsumptionForToday(long userId, long deviceCategoryId)
        {
            return SumEnergyConsumption(userId, DateTime.Today, deviceCategoryId);
        }

        public double GetUserEnergyConsumptionForThisMonth(long userId, long deviceCategoryId)
        {
            DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            return SumEnergyConsumption(userId, startOfMonth, deviceCategoryId);
        }

        public double GetUserEnergyConsumptionForThisYear(long userId, long deviceCategoryId)
        {
            DateTime startOfYear = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
            return SumEnergyConsumption(userId, startOfYear, deviceCategoryId);
        }

        public double SumEnergyConsumption(long userId, DateTime StartDate, long deviceCategoryId)
        {
            var devicesForUser = _context.Devices
                                .Include(d => d.DeviceModel)
                                    .ThenInclude(dm => dm.DeviceType)
                                    .ThenInclude(dt => dt.DeviceCategory)
                                .Where(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId)
                                .ToList();

            if (devicesForUser.Count == 0)
            {
                return 0;
            }

            var deviceIds = devicesForUser.Select(d => d.Id).ToList();
            var usageList = new List<DeviceEnergyUsage>();

            //var StartDate = DateTime.Today;
            var EndDate = DateTime.Now;

            usageList = _context.DeviceEnergyUsages
                        .Where(u => deviceIds.Contains(u.DeviceId) && u.StartTime >= StartDate && u.StartTime < EndDate/* && u.EndTime <= EndDate*/)
                        .ToList();

            var totalEnergyConsumption = 0.0;

            foreach (var device in devicesForUser)
            {
                var deviceUsageList = usageList.Where(u => u.Device.Id == device.Id).ToList();

                var DeviceModel = _context.DeviceModels.FirstOrDefault(dm => dm.Id == device.DeviceModelId);
                float EnergyInKwh = DeviceModel.EnergyKwh;

                foreach (var usage in deviceUsageList)
                {
                    if (usage.EndTime == null || usage.EndTime > EndDate)
                        usage.EndTime = EndDate;

                    TimeSpan timeDifference = (TimeSpan)(usage.EndTime - usage.StartTime);
                    double hours = Math.Abs(timeDifference.TotalHours);
                    totalEnergyConsumption += hours * EnergyInKwh;
                }
            }

            return Math.Round(totalEnergyConsumption, 2);
        }

        public List<EnergyToday> ProsumerElectricityUsageForTodayByHour(long userId, long deviceCategoryId)
        {
            var deviceIds = _context.Devices
                .Include(d => d.DeviceModel)
                .ThenInclude(dm => dm.DeviceType)
                .ThenInclude(dt => dt.DeviceCategory)
                .Where(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId)
                .Select(d => d.Id)
                .ToList();

            DateTime startOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime currentTime = DateTime.Now;

            List<DeviceEnergyUsage> UsageList = new List<DeviceEnergyUsage>();
            var Results = new List<EnergyToday>();

            UsageList = _context.DeviceEnergyUsages
                        .Where(u => deviceIds.Contains(u.DeviceId) && u.StartTime >= startOfDay && (u.EndTime <= currentTime || u.EndTime == null))
                        .ToList();

            for (var hour = startOfDay.Hour; hour <= currentTime.Hour; hour = hour + 1)
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

        // Device
        public DeviceTimeDTO FromWhenToWhenDeviceWorks(long deviceId, DateTime turnedOn, DateTime turnedOff)
        {
            if (turnedOn!=default(DateTime) || turnedOff!=default(DateTime))
            {
                bool checker = TurnOnOffDevice(deviceId, turnedOn, turnedOff);
                if (checker == false)
                    return null;
            }
            
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT deu.StartTime, 
	                                           CASE 
			                                        WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours')
                                                    WHEN deu.EndTime > datetime('now', '+2 hours') THEN datetime('now', '+2 hours')
			                                        ELSE deu.EndTime
	                                           END AS EndTime,
	                                           (julianday(CASE WHEN deu.EndTime IS NULL THEN datetime('now', '+2 hours') WHEN deu.EndTime > datetime('now', '+2 hours') THEN datetime('now', '+2 hours') ELSE deu.EndTime END) - julianday(deu.StartTime)) * 24.0 * 60 * 60 AS SecondsWorked
                                        FROM DeviceEnergyUsages deu 
                                        JOIN Devices d ON deu.DeviceId = d.Id AND deu.DeviceId = @deviceId
                                        WHERE deu.StartTime <= datetime('now', '+2 hours')
                                        ORDER BY deu.StartTime DESC
                                        LIMIT 1;";

                command.Parameters.Add(new SqliteParameter("@deviceId", deviceId));

                DeviceTimeDTO StartEndDuration = null;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime startDate = DateTime.ParseExact(reader["StartTime"].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            DateTime endDate = DateTime.ParseExact(reader["EndTime"].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                            double inTotalWorked = double.Parse(reader["SecondsWorked"].ToString());
                            int daysWorked = (int)inTotalWorked / (3600 * 24);
                            inTotalWorked -= daysWorked * (3600 * 24);
                            int hoursWorked = (int)inTotalWorked / 3600;
                            inTotalWorked -= hoursWorked * 3600;
                            int minutesWorked = (int)inTotalWorked / 60;
                            inTotalWorked -= minutesWorked * 60;
                            int secondsWorked = (int)inTotalWorked;

                            var startTime = startDate.ToString("dd-MM-yyyy HH:mm:ss");
                            var endTime = endDate.ToString("dd-MM-yyyy HH:mm:ss");
                            var duration = "";

                            if(hoursWorked<10 && minutesWorked<10 && secondsWorked<10)
                                duration = daysWorked + ":0" + hoursWorked + ":0" + minutesWorked + ":0" + secondsWorked;
                            else if (hoursWorked < 10 && minutesWorked<10)
                                duration = daysWorked + ":0" + hoursWorked + ":0" + minutesWorked + ":" + secondsWorked;
                            else if (hoursWorked<10 && secondsWorked<10)
                                duration = daysWorked + ":0" + hoursWorked + ":" + minutesWorked + ":0" + secondsWorked;
                            else if (minutesWorked<10 && secondsWorked<10)
                                duration = daysWorked + ":" + hoursWorked + ":0" + minutesWorked + ":0" + secondsWorked;
                            else if (hoursWorked<10)
                                duration = daysWorked + ":0" + hoursWorked + ":" + minutesWorked + ":" + secondsWorked;
                            else if (minutesWorked < 10)
                                duration = daysWorked + ":" + hoursWorked + ":0" + minutesWorked + ":" + secondsWorked;
                            else if (secondsWorked < 10)
                                duration = daysWorked + ":" + hoursWorked + ":" + minutesWorked + ":0" + secondsWorked;
                            else
                                duration = daysWorked + ":" + hoursWorked + ":" + minutesWorked + ":" + secondsWorked;

                            StartEndDuration = new DeviceTimeDTO
                            {
                                StartTime = startTime,
                                EndTime = endTime,
                                Duration = duration
                            };
                        }
                    }
                    else
                    {
                        StartEndDuration = new DeviceTimeDTO
                        {
                            StartTime = "/",
                            EndTime = "/",
                            Duration = "00:00:00"
                        };
                    }
                }

                return StartEndDuration;
            }
        }

        public bool TurnOnOffDevice(long deviceId, DateTime turnedOn, DateTime turnedOff)
        {
            using (var _connection = _context.Database.GetDbConnection())
            {
                _connection.Open();
                var command = _connection.CreateCommand();
                command.CommandText = @"
                                        SELECT TurnOn
                                        FROM Devices
                                        WHERE Id = @deviceId;
                                       ";

                command.Parameters.Add(new SqliteParameter("@deviceId", deviceId));
                int TurnOn = -1;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        TurnOn = reader.GetInt32(0);
                    }
                }
                
                if (TurnOn == 0)
                {
                    command.CommandText = @"
                                            INSERT INTO DeviceEnergyUsages (DeviceId, StartTime, EndTime)
                                                                    VALUES (@deviceId, @turnedOn, NULL);
                                            
                                            UPDATE Devices
                                            SET TurnOn = 1
                                            WHERE Id = @deviceId;
                                           ";
                }
                else
                {
                    command.CommandText = @"
                                            UPDATE DeviceEnergyUsages
                                            SET EndTime = @turnedOff
                                            WHERE DeviceId = @deviceId /*AND StartTime = @turnedOn*/ AND EndTime IS NULL;
                                           
                                            UPDATE Devices
                                            SET TurnOn = 0
                                            WHERE Id = @deviceId;
                                           ";
                }

                command.Parameters.Add(new SqliteParameter("@turnedOn", turnedOn));
                command.Parameters.Add(new SqliteParameter("@turnedOff", turnedOff));

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
