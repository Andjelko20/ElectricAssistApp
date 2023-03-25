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

        public Device addNewDevice(Device device)
        {
            var result = _context.Devices.Add(device);
            _context.SaveChanges();
            return device;
        }
    }
}
