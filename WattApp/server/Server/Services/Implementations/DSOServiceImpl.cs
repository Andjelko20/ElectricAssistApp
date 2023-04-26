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

                var usageTime = (usageEnd - usageStart).TotalHours;
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
            DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            DateTime endTime = DateTime.Now;

            var deviceTypeIds = _context.DeviceTypes
                .Where(dt => dt.CategoryId == deviceCategoryId)
                .Select(dt => dt.Id)
                .ToList();

            var deviceModelIds = _context.DeviceModels
                .Where(dm => deviceTypeIds.Contains(dm.DeviceTypeId))
                .Select(dm => dm.Id)
                .ToList();

            var devices = _context.Devices
                .Where(d => deviceModelIds.Contains(d.DeviceModelId))
                .Select(d => d.Id)
                .ToList();

            List<DeviceEnergyUsage> deviceEnergyUsages = _context.DeviceEnergyUsages
                            .Where(deu => devices.Contains(deu.DeviceId)
                                    && deu.StartTime >= startOfMonth && deu.StartTime <= endTime
                                    && _context.Devices.Any(d => d.Id == deu.DeviceId
                                        && _context.Users.Any(u => u.Id == d.UserId
                                            && _context.Settlements.Any(s => s.Id == u.SettlementId
                                                && s.CityId == cityId
                                                && _context.Cities.Any(c => c.Id == s.CityId)))))
                            .ToList();

            double totalUsage = 0.0;
            foreach (var usage in deviceEnergyUsages)
            {
                if (usage.EndTime == null || usage.EndTime > endTime)
                    usage.EndTime = endTime;

                var usageTime = (usage.EndTime - usage.StartTime).TotalHours;
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
                        energyUsage += reader.GetDouble(0);
                    }
                }
                return energyUsage;
            }
        }

        public List<EnergyToday> CalculateEnergyUsageForToday(long settlementId, long deviceCategoryId)
        {
            var Result = new List<EnergyToday>();
            var startDateTime = DateTime.Today;
            var endDateTime = DateTime.Now;

            // Pronađi sve uređaje iz datog naselja koji pripadaju datoj kategoriji uređaja
            var deviceIds = _context.Devices
                .Where(d => d.User.SettlementId == settlementId
                    && d.DeviceModel.DeviceType.CategoryId == deviceCategoryId)
                .Select(d => d.Id)
                .ToList();

            Console.WriteLine("************************ devices count = " + deviceIds.Count());

            // Pronadji sve zapise potrosnje iz tabele DeviceEnergyUsages za sve uredjaje koji su radili u zadatom vremenskom intervalu
            var energyUsages = _context.DeviceEnergyUsages
                .Where(usage => usage.StartTime >= startDateTime && usage.StartTime <= endDateTime
                    && deviceIds.Contains(usage.DeviceId))
                .ToList();

            Console.WriteLine("************************ energyUsages count = " + energyUsages.Count());

            // Sumiraj potrošnju svih uređaja po satu u zadatom vremenskom intervalu
            for (int hour = 0; hour <= endDateTime.Hour; hour++)
            {
                var startTime = startDateTime.AddHours(hour);
                var endTime = startDateTime.AddHours(hour + 1);
                double energyUsage = 0.0;
                //var energyUsageResult = energyUsages
                //    .Where(usage => usage.StartTime < endTime && (usage.EndTime == null || usage.EndTime > startTime))
                //    .Sum(usage => (endTime - (usage.StartTime < startTime ? startTime : usage.StartTime)).TotalHours * (usage.Device != null ? usage.Device.DeviceModel.EnergyKwh : 0));

                foreach (var usage in energyUsages)
                {
                    Console.WriteLine("*********************** usage.DeviceId="+usage.DeviceId+" --- usage.StartTime="+usage.StartTime+" --- usage.EndTime="+usage.EndTime);
                    double energyKwh = _context.DeviceEnergyUsages
                                        .Join(
                                            _context.Devices,
                                            usage => usage.DeviceId,
                                            device => device.Id,
                                            (usage, device) => new { Usage = usage, Device = device })
                                        .Where(joinResult => joinResult.Usage.DeviceId == usage.DeviceId)
                                        .Select(joinResult => joinResult.Device.DeviceModel.EnergyKwh)
                                        .FirstOrDefault();

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
                            overlapEnd = usage.EndTime;
                    }

                    if (overlapStart < overlapEnd)
                    {
                        var durationInHours = (overlapEnd - overlapStart).TotalHours;
                        energyUsage += durationInHours * energyKwh;
                    }
                }

                Result.Add(new EnergyToday
                {
                    EnergyUsageResult = Math.Round(energyUsage, 2),
                    Hour = startTime.Hour,
                    Day = DateTime.Now.Day,
                    Month = DateTime.Now.ToString("MMMM"),
                    Year = DateTime.Now.Year
                });
            }

            return Result;
        }

        public List<EnergyToday> CalculateEnergyUsageForTodayInCity(long cityId, long deviceCategoryId)
        {
            var Result = new List<EnergyToday>();
            var startDateTime = DateTime.Today;
            var endDateTime = DateTime.Now;

            // Pronađi sve uređaje iz datog naselja koji pripadaju datoj kategoriji uređaja
            var deviceIds = _context.Devices
                .Where(d => d.User.Settlement.CityId == cityId
                    && d.DeviceModel.DeviceType.CategoryId == deviceCategoryId)
                .Select(d => d.Id)
                .ToList();

            Console.WriteLine("************************ devices count = " + deviceIds.Count());

            // Pronadji sve zapise potrosnje iz tabele DeviceEnergyUsages za sve uredjaje koji su radili u zadatom vremenskom intervalu
            var energyUsages = _context.DeviceEnergyUsages
                .Where(usage => usage.StartTime >= startDateTime && usage.StartTime <= endDateTime
                    && deviceIds.Contains(usage.DeviceId))
                .ToList();

            Console.WriteLine("************************ energyUsages count = " + energyUsages.Count());

            // Sumiraj potrošnju svih uređaja po satu u zadatom vremenskom intervalu
            for (int hour = 0; hour <= endDateTime.Hour; hour++)
            {
                var startTime = startDateTime.AddHours(hour);
                var endTime = startDateTime.AddHours(hour + 1);
                double energyUsage = 0.0;
                //var energyUsageResult = energyUsages
                //    .Where(usage => usage.StartTime < endTime && (usage.EndTime == null || usage.EndTime > startTime))
                //    .Sum(usage => (endTime - (usage.StartTime < startTime ? startTime : usage.StartTime)).TotalHours * (usage.Device != null ? usage.Device.DeviceModel.EnergyKwh : 0));

                foreach (var usage in energyUsages)
                {
                    Console.WriteLine("*********************** usage.DeviceId=" + usage.DeviceId + " --- usage.StartTime=" + usage.StartTime + " --- usage.EndTime=" + usage.EndTime);
                    double energyKwh = _context.DeviceEnergyUsages
                                        .Join(
                                            _context.Devices,
                                            usage => usage.DeviceId,
                                            device => device.Id,
                                            (usage, device) => new { Usage = usage, Device = device })
                                        .Where(joinResult => joinResult.Usage.DeviceId == usage.DeviceId)
                                        .Select(joinResult => joinResult.Device.DeviceModel.EnergyKwh)
                                        .FirstOrDefault();

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
                            overlapEnd = usage.EndTime;
                    }

                    if (overlapStart < overlapEnd)
                    {
                        var durationInHours = (overlapEnd - overlapStart).TotalHours;
                        energyUsage += durationInHours * energyKwh;
                    }
                }

                Result.Add(new EnergyToday
                {
                    EnergyUsageResult = Math.Round(energyUsage, 2),
                    Hour = startTime.Hour,
                    Day = DateTime.Now.Day,
                    Month = DateTime.Now.ToString("MMMM"),
                    Year = DateTime.Now.Year
                });
            }

            return Result;
        }
    }
}
