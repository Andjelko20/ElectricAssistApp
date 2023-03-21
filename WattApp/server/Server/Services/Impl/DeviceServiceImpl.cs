using Server.Data;
using Server.Models;

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
            _context.SaveChanges();
            return device;
        }

        public Device changeTurnOnStatus(long deviceId)
        {
            Device device = _context.Devices.Find(deviceId);
            device.TurnOn = !device.TurnOn;
            _context.Devices.Update(device);
            _context.SaveChanges();
            return device;

        }

        public Device deleteDeviceById(long id)
        {
            Device device = _context.Devices.Find(id);
            var result = _context.Devices.Remove(device);
            _context.SaveChanges();
            return device;
        }

        public Device editDevice(Device device)
        {
            _context.Devices.Update(device);
            _context.SaveChanges();
            return device;
        }

        public List<Device> getAllDevices()
        {
            return _context.Devices.ToList();
        }

        public Device getDeviceById(long deviceId)
        {
            return _context.Devices.Find(deviceId);
        }

    }
}
