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

        public List<DeviceEnergyUsage> GetUsageHistoryForDeviceInLastYear(int deviceId)
        {
            DateTime OneYearAgo = DateTime.Now.AddYears(-1); // tip DATETIME, trenutna godina 2023. -1 = 2022.
            
            return _context.DeviceEnergyUsages
            .Where(u => u.DeviceId == deviceId && u.StartTime >= OneYearAgo)
            .OrderBy(u => u.StartTime)
            .ToList();
        }

        public List<DeviceEnergyUsage> GetUsageHistoryForDeviceInLastMonth(int deviceId)
        {
            DateTime OneMonthAgo = DateTime.Now.AddMonths(-1);

            return _context.DeviceEnergyUsages
            .Where(u => u.DeviceId == deviceId && u.StartTime >= OneMonthAgo)
            .OrderBy(u => u.StartTime)
            .ToList();
        }

        public List<DeviceEnergyUsage> GetUsageHistoryForDeviceInLastDay(int deviceId)
        {
            DateTime ADayAgo = DateTime.Now.AddDays(-1);

            return _context.DeviceEnergyUsages
            .Where(u => u.DeviceId == deviceId && u.StartTime >= ADayAgo)
            .OrderBy(u => u.StartTime)
            .ToList();
        }
    }
}
