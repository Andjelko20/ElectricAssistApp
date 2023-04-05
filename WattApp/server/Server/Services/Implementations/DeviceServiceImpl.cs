﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using MimeKit.Encodings;
using Server.Data;
using Server.DTOs;
using Server.Exceptions;
using Server.Filters;
using Server.Models;
using Server.Models.DropDowns.Devices;
using System.Reflection;

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
            /*
            if (device.EnergyInKwh == null || device.EnergyInKwh == 0)
            {
                device.EnergyInKwh = _context.TypeBrandModels.FirstOrDefault(x => x.TypeId == device.DeviceTypeId && x.ModelId == device.DeviceModelId && x.BrandId == x.BrandId).EnergyKwh;
            }
            if (device.StandByKwh == null || device.StandByKwh == 0)
            {
                device.StandByKwh = _context.TypeBrandModels.FirstOrDefault(x => x.TypeId == device.DeviceTypeId && x.ModelId == device.DeviceModelId && x.BrandId == x.BrandId).StandByKwh;
            }
			*/
            long deviceModelId = device.DeviceModelId;

            if (device.EnergyInKwh == null || device.EnergyInKwh == 0)
            {
                var result = _context.DeviceModels.Find(deviceModelId);
                if (result == null)
                {
                    throw new ItemNotFoundException("Wrong device model id!");
                }
                device.EnergyInKwh = result.EnergyKwh;
            }
            if (device.StandByKwh == null || device.StandByKwh == 0)
            {
                var result = _context.DeviceModels.Find(deviceModelId);
                if(result == null)
                {
                    throw new ItemNotFoundException("Wrong device model id!");
                }
                device.StandByKwh = (float)result.StandByKwh;
            }

            _context.Devices.Add(device);
            _context.SaveChanges();
            return device;
        }
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
        /// <inheritdoc/>
        public Device deleteDeviceById(long id, long userId)
        {
            Device device = _context.Devices.FirstOrDefault(src => src.Id == id && src.UserId == userId);
            var result = _context.Devices.Remove(device);
            _context.SaveChanges();
            return device;
        }
        /// <inheritdoc/>
        public Device editDevice(Device device, long userId)
        {
            Device response = _context.Devices.FirstOrDefault(src => src.Id == device.Id && src.UserId == userId);

            response.Name = device.Name;

            response.EnergyInKwh = device.EnergyInKwh;
            if (response.EnergyInKwh == null || response.EnergyInKwh == 0)
            {
                var result = _context.DeviceModels.Find(response.DeviceModelId);
                if (result == null)
                {
                    throw new ItemNotFoundException("Wrong device model id!");
                }
                response.EnergyInKwh = (float)result.EnergyKwh;
            }

            response.StandByKwh = device.StandByKwh;
            if (response.StandByKwh == null || response.StandByKwh == 0)
            {
                var result = _context.DeviceModels.Find(response.DeviceModelId);
                if (result == null)
                {
                    throw new ItemNotFoundException("Wrong device model id!");
                }
                response.StandByKwh = (float)result.StandByKwh;
            }

            response.Visibility = device.Visibility;
            if(response.Visibility == false)
            {
                response.Controlability = false;
            }
            else
            {
                response.Controlability = device.Controlability;
            }
            //response.Controlability = device.Controlability;
            response.TurnOn = device.TurnOn;

            _context.Devices.Update(response);
            _context.SaveChanges();
            return response;
        }


        /*public List<Device> getAllDevices()
        {
                return _context.Devices.Where(src => src.Visibility == true).ToList();
        }*/

        /// <inheritdoc/>
        public Device getDeviceById(long deviceId)
        {
            return _context.Devices.FirstOrDefault(src => src.Id == deviceId && src.Visibility == true);
        }
        /// <inheritdoc/>
        public List<Device> getMyDevices(long userId, DeviceFilterModel deviceFilter)
        {
            //return _context.Devices.Where(src => src.UserId == userId).ToList();

            IQueryable<Device> query = _context.Devices.Where(src => src.UserId == userId);
            if(deviceFilter != null)
            {
                query = DeviceFilter.ApplyFilter(query, deviceFilter);
            }
            List<Device> devices = query.ToList();
            return devices;
        }
        /// <inheritdoc/>
        public Device getYourDeviceById(long deviceId, long userId)
        {
            return _context.Devices.FirstOrDefaultAsync(src => src.Id == deviceId && src.UserId == userId).Result;
        }
        /// <inheritdoc/>
        public List<Device> getUserDevices(long userId)
        {
            return _context.Devices.Where(src => src.UserId == userId && src.Visibility == true).ToList();
        }
    }
}
