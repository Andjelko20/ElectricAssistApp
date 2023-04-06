using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using MimeKit.Encodings;
using Polly;
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
        DeviceModelService _deviceModelService;
        IMapper _mapper;
        public DeviceServiceImpl(SqliteDbContext context, DeviceModelService deviceModelService, IMapper mapper)
        {
            _context = context;
            _deviceModelService = deviceModelService;   
            _mapper = mapper;
        }

        private void formatDeviceResponseDTO(ref DeviceResponseDTO responseDTO, long id)
        {
            var _connection = _context.Database.GetDbConnection();
            if (_connection != null)
            {
                _connection.Open();

                var command = _connection.CreateCommand();

                //DeviceCategory
                command.CommandText = @" select Name 
                                             from DeviceCategories 
                                             where Id in (
                                                          select CategoryId
                                                          from DeviceTypes
                                                          where Id in (
                                                                        select DeviceTypeId 
                                                                        from DeviceModels 
                                                                        where Id = @id))";
                command.Parameters.Add(new SqliteParameter("@id", id));
                var deviceCategory = command.ExecuteScalar().ToString();
                responseDTO.DeviceCategory = deviceCategory;

                //DeviceType
                command.CommandText = @"select Name 
                                            from DeviceTypes 
                                            where Id in (
                                                         select DeviceTypeId 
                                                         from DeviceModels 
                                                         where Id = @id)";
                //command.Parameters.Add(new SqliteParameter("@id", id));
                var deviceType = command.ExecuteScalar().ToString();
                responseDTO.DeviceType = deviceType;

                //DeviceBrand
                command.CommandText = @"select Name 
                                            from DeviceTypes 
                                            where Id in (
                                                         select DeviceTypeId 
                                                         from DeviceModels 
                                                         where Id = @id)";
                //command.Parameters.Add(new SqliteParameter("@id", id));
                var deviceBrand = command.ExecuteScalar().ToString();
                responseDTO.DeviceBrand = deviceBrand;

                _connection.Close();
            }

            responseDTO.DeviceModel = _deviceModelService.getModelNameById(id);
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
        public DataPage<DeviceResponseDTO> getMyDevices(long userId, DeviceFilterModel deviceFilter, int pageNumber, int pageSize)
        {
            //return _context.Devices.Where(src => src.UserId == userId).ToList();

            IQueryable<Device> query = _context.Devices.Where(src => src.UserId == userId);
            if(query.Count() == 0) throw new HttpRequestException("There is no devices!", null, System.Net.HttpStatusCode.NotFound);

            if(deviceFilter != null)
            {
                query = DeviceFilter.ApplyFilter(query, deviceFilter);
            }

            int maxPageNumber;
            if (query.Count() % pageSize == 0) maxPageNumber = query.Count() / pageSize;
            else maxPageNumber = query.Count() / pageSize + 1;

            if (pageNumber < 1 || pageNumber > maxPageNumber) throw new HttpRequestException("Invalid page number!", null, System.Net.HttpStatusCode.BadRequest);
            if (pageSize < 1) throw new HttpRequestException("Invalid page size number!", null, System.Net.HttpStatusCode.BadRequest);

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            List<Device> devices = query.ToList();
            List<DeviceResponseDTO> deviceResponseDTOs = new List<DeviceResponseDTO>();

            foreach(Device device in devices)
            {
                DeviceResponseDTO deviceResponseDTO = _mapper.Map<DeviceResponseDTO>(device);
                formatDeviceResponseDTO(ref deviceResponseDTO, device.DeviceModelId);
                deviceResponseDTOs.Add(deviceResponseDTO);
            }

            DataPage <DeviceResponseDTO> page = new();
            page.Data = deviceResponseDTOs;
            page.NumberOfPages = maxPageNumber;
            page.PreviousPage = (pageNumber - 1 == 0) ? null : pageNumber - 1;
            page.NextPage = (pageNumber == maxPageNumber) ? null : pageNumber + 1;

            return page;
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
