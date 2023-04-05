using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using Server.Data;
using Server.DTOs;
using Server.Exceptions;
using Server.Models;
using Server.Services;
using System.Net;
using System.Reflection;
using System.Security.Claims;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly SqliteDbContext context;

        DeviceService _deviceService;
        DeviceCategoryService _deviceCategoryService;
        DeviceTypeService _deviceTypeService;
        DeviceBrandService _deviceBrandService;
        DeviceModelService _deviceModelService;
        IMapper _mapper;

        public DeviceController(SqliteDbContext context, DeviceService deviceService, DeviceCategoryService deviceCategoryService, DeviceTypeService deviceTypeService, DeviceBrandService deviceBrandService, DeviceModelService deviceModelService, IMapper mapper)
        {
            this.context = context;
            _deviceService = deviceService;
            _deviceCategoryService = deviceCategoryService;
            _deviceTypeService = deviceTypeService;
            _deviceBrandService = deviceBrandService;
            _deviceModelService = deviceModelService;
            _mapper = mapper;
        }
        /// <summary>
        /// Format deviceResponseDTO -> get category, type, brand and model name 
        /// </summary>
        /// <param name="responseDTO">responseDTO that you want to format</param>
        /// <param name="id">model id</param>
        private void formatDeviceResponseDTO(ref DeviceResponseDTO responseDTO, long id)
        {
            var _connection = context.Database.GetDbConnection();
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

        /// <summary>
        /// Get device by device id (DSO if he has permissions, for PROSUMER if its his device)
        /// </summary>
        /// 
        [ProducesResponseType(typeof(DeviceResponseDTO), 200)]
        [HttpGet("{id:long}")]
        [Authorize(Roles = "dispatcher, prosumer")]
        public IActionResult getDeviceById([FromRoute]long id)
        {
            try
            {
                DeviceResponseDTO responseDTO;
                Device device;
                long modelId;
                if (User.IsInRole("prosumer"))
                {
                    var credentials = HttpContext.User.Identity as ClaimsIdentity;
                    long userId = long.Parse(credentials.FindFirst(ClaimTypes.Actor).Value);

                    device = _deviceService.getYourDeviceById(id, userId);
                    responseDTO = _mapper.Map<DeviceResponseDTO>(device);
                }
                else
                {
                    device = _deviceService.getDeviceById(id);
                    modelId = device.DeviceModelId;
                    responseDTO = _mapper.Map<DeviceResponseDTO>(device);
                }

                if (responseDTO == null)
                {
                    throw new ItemNotFoundException("Device not found!");
                }
                formatDeviceResponseDTO(ref responseDTO, device.DeviceModelId);
                return Ok(responseDTO);
            }
            catch (ItemNotFoundException ex)
            {
                return NotFound(new
                {
                    title = "Device not found!",
                    status = 404,
                    message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    title = "Internal server error!",
                    status = 500,
                    message = "An error occurred while processing your request."
                });
            }
            
        }
        /// <summary>
        /// Get all devices from user (for DSO)
        /// </summary>
        [HttpGet("devices{userId:long}")]
        [Authorize(Roles = "dispatcher")]
        public IActionResult getUserDevices([FromRoute]long userId)
        {
            try
            {
                List<Device> devices = _deviceService.getUserDevices(userId);
                if (devices == null) throw new ItemNotFoundException("Devices not found!");

                List<DeviceResponseDTO> deviceResponseDTOs = new List<DeviceResponseDTO>();
                foreach(Device device in devices)
                {
                    DeviceResponseDTO responseDTO = _mapper.Map<DeviceResponseDTO>(device);

                    formatDeviceResponseDTO(ref responseDTO, device.DeviceModelId);

                    deviceResponseDTOs.Add(responseDTO);
                }
                return Ok(deviceResponseDTOs);
            }
            catch(ItemNotFoundException ex)
            {
                return NotFound(new
                {
                    title = "Devices not found!",
                    status = 404,
                    message = ex.Message
                }) ;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    title = "Internal server error!",
                    status = 500,
                    message = "An error occurred while processing your request."
                });
            }
        }



        /// <summary>
        /// Get all devices (for PROSUMER)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "prosumer")]
        public IActionResult getAllDevices()
        {
            try
            {
                List<Device> devices = new List<Device>();
                List<DeviceResponseDTO> responseDTOs = new List<DeviceResponseDTO>();

                var credentials = HttpContext.User.Identity as ClaimsIdentity;
                long userId = long.Parse(credentials.FindFirst(ClaimTypes.Actor).Value);

                devices = _deviceService.getMyDevices(userId);
                if (devices == null || devices.Count == 0)
                {
                    throw new ItemNotFoundException("Devices not found!");
                }

                foreach (Device device in devices)
                {
                    DeviceResponseDTO responseDTO = _mapper.Map<DeviceResponseDTO>(device);

                    responseDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(responseDTO.DeviceCategory));
                    responseDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(responseDTO.DeviceType));
                    responseDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(responseDTO.DeviceBrand));
                    responseDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(responseDTO.DeviceModel));

                    responseDTOs.Add(responseDTO);
                }

                return Ok(responseDTOs);
            }
            catch (ItemNotFoundException ex)
            {
                return NotFound(new
                {
                    title = "Devices not found!",
                    status = 404,
                    message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    title = "Internal server error!",
                    status = 500,
                    message = "An error occurred while processing your request."
                });
            }

        }

       

        /// <summary>
        /// Add new device if you are PROSUMER or GUEST
        /// </summary>

        [HttpPost]
        [Authorize(Roles = "prosumer, guest")]
        public IActionResult addNewDevice([FromBody]DeviceCreateDTO deviceCreateDTO)
        {
            if(deviceCreateDTO == null)
            {
                throw new ArgumentNullException(nameof(deviceCreateDTO));
            }
            Device device = _mapper.Map<Device>(deviceCreateDTO);
            try
            {
                Device response = _deviceService.addNewDevice(device);
                if(response == null)
                {
                    throw new DbUpdateException("An error occurred while adding device! Please try again.");
                }
                var id = response.DeviceModelId;
                DeviceResponseDTO responseDTO = _mapper.Map<DeviceResponseDTO>(response);
                formatDeviceResponseDTO(ref responseDTO, id);

                return Ok(responseDTO);
            }
            catch(DbUpdateException ex)
            {
                return BadRequest(new
                {
                    title = "An error occurred!", 
                    status = 400, 
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    title = "Internal server error!",
                    status = 500,
                    message = "An error occurred while processing your request."
                });
            }
        }

        

        /// <summary>
        /// Change turn on/off status of device (DSO + PROSUMER)
        /// </summary>
        [HttpPut("turnOn{deviceId:long}")]
        [Authorize(Roles = "dispatcher, prosumer")]
        public IActionResult changeTurnOnStatus([FromRoute]long deviceId)
        {
            Device device = new Device();
            try
            {
                if (User.IsInRole("prosumer"))
                {
                    var credentials = HttpContext.User.Identity as ClaimsIdentity;
                    long userId = long.Parse(credentials.FindFirst(ClaimTypes.Actor).Value);

                    device = _deviceService.changeTurnOnStatus(deviceId, userId);
                }
                else
                {
                    device = _deviceService.changeTurnOnStatus(deviceId);
                }

                if(device == null)
                {
                    throw new DbUpdateException("An error occurred while processing your request.");
                }


                return Ok(_mapper.Map<DeviceResponseDTO>(device));
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    title = "Error occurred!",
                    status = 400,
                    message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    title = "Internal server error!",
                    status = 500,
                    message = "An error occurred while processing your request."
                });
            }


        }



        /*[HttpPut]
        [Authorize(Roles = "prosumer")]
        public IActionResult editDevice([FromBody]DeviceRequestDTO deviceRequestDTO)
        {
            try
            {
                DeviceResponseDTO deviceDTO = _mapper.Map<DeviceResponseDTO>(_deviceService.editDevice(_mapper.Map<Device>(deviceRequestDTO)));
                if(deviceDTO == null)
                {
                    throw new DbUpdateException("An error occurred while processing your request.");
                }

                deviceDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(deviceDTO.DeviceCategory));
                deviceDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(deviceDTO.DeviceType));
                deviceDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(deviceDTO.DeviceBrand));
                deviceDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(deviceDTO.DeviceModel));

                return Ok(deviceDTO);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    title = "Error occurred!",
                    status = 400,
                    message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    title = "Internal server error!",
                    status = 500,
                    message = "An error occurred while processing your request."
                });
            }

        }*/

        [HttpDelete("{id:long}")]
        public IActionResult deleteDeviceById([FromRoute]long id)
        {
            try
            {
                DeviceResponseDTO responseDTO = _mapper.Map<DeviceResponseDTO>(_deviceService.deleteDeviceById(id));
                if (responseDTO == null)
                {
                    throw new DbUpdateException("An error occurred while processing your request.");
                }
                return Ok(responseDTO);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    title = "Error occurred!",
                    status = 400,
                    message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    title = "Internal server error!",
                    status = 500,
                    message = "An error occurred while processing your request."
                });
            }

        }


        /// <summary>
        /// Change controlability status of device (DSO + PROSUMER)
        /// </summary>
        [HttpPut("controlability{deviceId:long}")]
        [Authorize(Roles = "prosumer")]
        public IActionResult changeDeviceControlability([FromRoute]long deviceId)
        {
            try
            {
                var credentials = HttpContext.User.Identity as ClaimsIdentity;
                long userId = long.Parse(credentials.FindFirst(ClaimTypes.Actor).Value);

                DeviceResponseDTO deviceDTO = _mapper.Map<DeviceResponseDTO>(_deviceService.changeDeviceControlability(deviceId, userId));
                if (deviceDTO == null)
                {
                    throw new DbUpdateException("An error occurred while processing your request.");
                }
               
                deviceDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(deviceDTO.DeviceCategory));
                deviceDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(deviceDTO.DeviceType));
                deviceDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(deviceDTO.DeviceBrand));
                deviceDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(deviceDTO.DeviceModel));

                return Ok(deviceDTO);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    title = "Error occurred!",
                    status = 400,
                    message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    title = "Internal server error!",
                    status = 500,
                    message = "An error occurred while processing your request."
                });
            }
        }

        /// <summary>
        /// Change visibility status of device (DSO + PROSUMER)
        /// </summary>
        [HttpPut("visibility{deviceId:long}")]
        [Authorize(Roles = "prosumer")]
        public IActionResult changeDeviceVisibility([FromRoute] long deviceId)
        {
            try
            {
                var credentials = HttpContext.User.Identity as ClaimsIdentity;
                long userId = long.Parse(credentials.FindFirst(ClaimTypes.Actor).Value);

                DeviceResponseDTO deviceDTO = _mapper.Map<DeviceResponseDTO>(_deviceService.changeDeviceVisibility(deviceId, userId));
                if (deviceDTO == null)
                {
                    throw new DbUpdateException("An error occurred while processing your request.");
                }

                deviceDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(deviceDTO.DeviceCategory));
                deviceDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(deviceDTO.DeviceType));
                deviceDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(deviceDTO.DeviceBrand));
                deviceDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(deviceDTO.DeviceModel));

                return  Ok(deviceDTO);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    title = "Error occurred!",
                    status = 400,
                    message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    title = "Internal server error!",
                    status = 500,
                    message = "An error occurred while processing your request."
                });
            }

        }
    }
}
