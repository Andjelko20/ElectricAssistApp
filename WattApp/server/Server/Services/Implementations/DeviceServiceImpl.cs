using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Server.Data;
using Server.DTOs;
using Server.Models;

namespace Server.Services.Implementations
{

    public class DeviceServiceImpl : DeviceService
    {
        SqliteDbContext _context;
        public DeviceServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }
        /// <inheritdoc/>
        public Device addNewDevice(Device device)
        {
            if (device.EnergyInKwh == null || device.EnergyInKwh == 0)
            {
                device.EnergyInKwh = _context.TypeBrandModels.FirstOrDefault(x => x.TypeId == device.DeviceTypeId && x.ModelId == device.DeviceModelId && x.BrandId == x.BrandId).EnergyKwh;
            }
            if (device.StandByKwh == null || device.StandByKwh == 0)
            {
                device.StandByKwh = _context.TypeBrandModels.FirstOrDefault(x => x.TypeId == device.DeviceTypeId && x.ModelId == device.DeviceModelId && x.BrandId == x.BrandId).StandByKwh;
            }
            _context.Devices.Add(device);
            _context.SaveChanges();
            return device;
        }

        public Device changeDeviceVisibility(long deviceId, long userId)
        {
            Device device = _context.Devices.FirstOrDefaultAsync(x => x.Id == deviceId && x.UserId == userId).Result;
            if (device != null)
            {
                device.Visibility = !(device.Visibility);
                if(device.Visibility == false)
                {
                    device.Controlability = false;
                }
                _context.Devices.Update(device);
                _context.SaveChangesAsync();
            }
            return device;
        }

        public Device changeDeviceControlability(long deviceId, long userId)
        {
            Device device = _context.Devices.FirstOrDefaultAsync(x => x.Id == deviceId && x.UserId == userId).Result;
            if (device != null)
            {
                if (device.Visibility == false)
                    device.Visibility = true;
                device.Controlability = !(device.Controlability);
                _context.Devices.Update(device);
                _context.SaveChangesAsync();
            }
            return device;
        }

        /// <inheritdoc/>
        public Device changeTurnOnStatus(long deviceId)
        {
            Device device = _context.Devices.FirstOrDefaultAsync(x => x.Id == deviceId && x.Controlability == true).Result;
            if (device != null)
            {
                device.TurnOn = !(device.TurnOn);
                _context.Devices.Update(device);
                _context.SaveChangesAsync();
                return device;
            }
                
            return device;
        }
        /// <inheritdoc/>
        public Device changeTurnOnStatus(long deviceId, long userId)
        {
            Device device = _context.Devices.FirstOrDefaultAsync(x => x.Id == deviceId && x.UserId == userId).Result;
            if(device != null)
            {
                device.TurnOn = !(device.TurnOn);
                _context.Devices.Update(device);
                _context.SaveChangesAsync();
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

        public List<Device> getAllDevices()
        {
                return _context.Devices.Where(src => src.Visibility == true).ToList();
        }

        public List<Device> getAllUsersDevices(long userId)
        {
            return _context.Devices.Where(src => src.UserId == userId).ToList();
        }
        public Device getDeviceById(long deviceId)
        {
            return _context.Devices.FirstOrDefault(src => src.Id == deviceId && src.Visibility == true);
        }

        public List<Device> getUsersDevices(long userId)
        {
            return _context.Devices.Where(src => src.UserId == userId).ToList();
        }

        public Device getYourDeviceById(long deviceId, long userId)
        {
            return _context.Devices.FirstOrDefaultAsync(src => src.Id == deviceId && src.UserId == userId).Result;
        }
    }
}
