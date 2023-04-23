using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;
using System.Collections.Generic;

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
            DateTime startOfYear = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
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
                                    && deu.StartTime >= startOfYear  && deu.StartTime <= endTime
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

                var usageTime = Math.Abs((usage.EndTime - usage.StartTime).TotalHours);
                var deviceEnergyUsage = _context.Devices
                    .Include(d => d.DeviceModel)
                    .Where(d => d.Id == usage.DeviceId)
                    .Select(d => d.DeviceModel.EnergyKwh)
                    .FirstOrDefault();

                totalUsage += deviceEnergyUsage * usageTime;
            }

            //return Math.Round(totalUsage, 2);
            return totalUsage;
        }
    }
}
