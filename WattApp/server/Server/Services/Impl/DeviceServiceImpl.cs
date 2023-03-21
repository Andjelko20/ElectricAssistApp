using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Server.Data;
using Server.DTOs;
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

        public Device changeTurnOnStatus(long deviceId, UserCheckDTO userCheck)
        {
            if (userCheck.RoleId == 1 || userCheck.RoleId == 2 || userCheck.RoleId == 5)
            {
                Device device = _context.Devices.FirstOrDefault(x => x.Id == deviceId && x.Controlability == true);
                device.TurnOn = !device.TurnOn;
                _context.SaveChanges();
                return device;
            }
            else
            {
                Device device = _context.Devices.FirstOrDefault(x => x.UserId == userCheck.UserId && x.Id == deviceId);
                device.TurnOn = !device.TurnOn;
                _context.SaveChanges();
                return device;
            }
            
            return null;

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

        public List<Device> getAllDevices(long roleId)
        {
            if(roleId == 1 || roleId == 2 || roleId == 5)
                return _context.Devices.Where(src => src.Visibility == true).ToList();
            return null;
        }

        public List<Device> getAllUsersDevices(long userId)
        {
            return _context.Devices.Where(src => src.UserId == userId).ToList();
        }

        public List<Device> getAllUsersDevices(long userId, long roleId)
        {
            throw new NotImplementedException();
        }

        public Device getDeviceById(long deviceId)
        {
            return _context.Devices.Find(deviceId);
        }

    }
}
