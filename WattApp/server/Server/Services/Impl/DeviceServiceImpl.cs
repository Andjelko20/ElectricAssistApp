using Server.Data;
using Server.Models;
using System.Diagnostics;

namespace Server.Services.Impl
{
    public class DeviceServiceImpl : DeviceService
    {
        SqliteDbContext _context;
        public DeviceServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public Device addNewDevice(Device device)
        {
            var result = _context.Devices.Add(device);
            if (result != null)
                return device;
            return null;
        }

        public Device getDeviceById(long deviceId)
        {
            Device device = _context.Devices.FindAsync(deviceId).Result;
            return device;
        }
    }
}
