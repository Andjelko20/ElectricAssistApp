using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Services.Implementations
{
    public class HistoryServiceImpl : IHistoryService
    {
        private readonly SqliteDbContext _context;
        public HistoryServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public double GetUsageHistoryForDeviceInLastYear(int deviceId)
        {
            DateTime OneYearAgo = DateTime.Now.AddYears(-1); // tip DATETIME, trenutna godina 2023. -1 = 2022.

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= OneYearAgo)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            var device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            float EnergyInKwh = -1;
            if (device != null)
                EnergyInKwh = device.EnergyInKwh;

            double Consumption = 0.0;
            double Hours = -1;
            // prodjem kroz tu listu i vidim koliko sati je radio
            foreach (var item in deviceEnergyUsageLista)
            {
                Hours = (item.EndTime - item.StartTime).TotalHours;
                Consumption += (double)Math.Round(EnergyInKwh * Hours, 2);
            }

            return Consumption;
        }

        public double GetUsageHistoryForDeviceInLastMonth(int deviceId)
        {
            DateTime OneMonthAgo = DateTime.Now.AddMonths(-1);

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= OneMonthAgo)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            var device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            float EnergyInKwh = -1;
            if (device != null)
                EnergyInKwh = device.EnergyInKwh;

            double Consumption = 0.0;
            double Hours = -1;
            // prodjem kroz tu listu i vidim koliko sati je radio
            foreach (var item in deviceEnergyUsageLista)
            {
                Hours = (item.EndTime - item.StartTime).TotalHours;
                Consumption += (double)Math.Round(EnergyInKwh * Hours, 2);
            }

            return Consumption;
        }

        public double GetUsageHistoryForDeviceInLastDay(int deviceId)
        {
            var device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            float EnergyInKwh = -1;
            if (device != null)
                EnergyInKwh = device.EnergyInKwh;
            else
                return EnergyInKwh; // kada uredjaj ne postoji vrati -1

            DateTime ADayAgo = DateTime.Now.AddDays(-1);

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= ADayAgo)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();
            double Consumption = 0.0;
            double Hours = -1;
            foreach (var item in deviceEnergyUsageLista)
            {
                Hours = (item.EndTime - item.StartTime).TotalHours;
                Consumption += (double)Math.Round(EnergyInKwh * Hours, 2);
            }

            return Consumption;
        }
    }
}
