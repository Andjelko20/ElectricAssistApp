using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
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
        DeviceService _deviceService;
        DeviceCategoryService _deviceCategoryService;
        DeviceTypeService _deviceTypeService;
        DeviceBrandService _deviceBrandService;
        DeviceModelService _deviceModelService;
        IMapper _mapper;

        public DeviceController(DeviceService deviceService, DeviceCategoryService deviceCategoryService, DeviceTypeService deviceTypeService, DeviceBrandService deviceBrandService, DeviceModelService deviceModelService, IMapper mapper)
        {
            _deviceService = deviceService;
            _deviceCategoryService = deviceCategoryService;
            _deviceTypeService = deviceTypeService;
            _deviceBrandService = deviceBrandService;
            _deviceModelService = deviceModelService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get device details by device id. 
        /// </summary>
        /// 
        [ProducesResponseType(typeof(DeviceResponseDTO), 200)]
        [HttpGet("{id:long}")]
        [Authorize(Roles = "dispecer, prosumer")]
        public IActionResult getDeviceById([FromRoute]long id)
        {
            try
            {
                DeviceResponseDTO deviceDTO = new DeviceResponseDTO();
                if (User.IsInRole("prosumer"))
                {
                    var credentials = HttpContext.User.Identity as ClaimsIdentity;
                    int userId = int.Parse(credentials.FindFirst(ClaimTypes.Actor).Value);

                    deviceDTO = _mapper.Map<DeviceResponseDTO>(_deviceService.getYourDeviceById(id, userId));
                }
                else
                {
                    deviceDTO = _mapper.Map<DeviceResponseDTO>(_deviceService.getDeviceById(id));
                }

                if (deviceDTO == null)
                {
                    throw new ItemNotFoundException("Device for not found!");
                }

                deviceDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(deviceDTO.DeviceCategory));
                deviceDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(deviceDTO.DeviceType));
                deviceDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(deviceDTO.DeviceBrand));
                deviceDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(deviceDTO.DeviceModel));

                return Ok(deviceDTO);
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

        [HttpGet("devices{userId:long}")]
        [Authorize(Roles = "dispecer")]
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

                    responseDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(responseDTO.DeviceCategory));
                    responseDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(responseDTO.DeviceType));
                    responseDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(responseDTO.DeviceBrand));
                    responseDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(responseDTO.DeviceModel));

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
        /// Get all visible devices for every user if you are DSO and only your devices if you are PROSUMER.
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
                int userId = int.Parse(credentials.FindFirst(ClaimTypes.Actor).Value);

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

        //IActionResult

        [HttpPost]
        [Authorize(Roles = "prosumer, guest")]
        public IActionResult addNewDevice([FromBody]DeviceRequestDTO deviceRequestDTO)
        {
            try
            {
                DeviceResponseDTO deviceDTO = _mapper.Map<DeviceResponseDTO>(_deviceService.addNewDevice(_mapper.Map<Device>(deviceRequestDTO)));
                if (deviceDTO == null)
                {
                    throw new DbUpdateException("An error occurred while adding device! Please try again.");
                }

                deviceDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(deviceDTO.DeviceCategory));
                deviceDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(deviceDTO.DeviceType));
                deviceDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(deviceDTO.DeviceBrand));
                deviceDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(deviceDTO.DeviceModel));

                return Ok(deviceDTO);
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

        

        [HttpPut("turnOn{deviceId:long}")]
        [Authorize(Roles = "dispecer, prosumer")]
        public IActionResult changeTurnOnStatus([FromRoute]long deviceId)
        {
            Device device = new Device();
            try
            {
                if (User.IsInRole("prosumer"))
                {
                    var credentials = HttpContext.User.Identity as ClaimsIdentity;
                    int userId = int.Parse(credentials.FindFirst(ClaimTypes.Actor).Value);

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



        [HttpPut]
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

        }

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

        [HttpPut("controlability{deviceId:long}")]
        [Authorize(Roles = "prosumer")]
        public IActionResult changeDeviceControlability([FromRoute]long deviceId)
        {
            try
            {
                var credentials = HttpContext.User.Identity as ClaimsIdentity;
                int userId = int.Parse(credentials.FindFirst(ClaimTypes.Actor).Value);

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

        [HttpPut("visibility{deviceId:long}")]
        [Authorize(Roles = "prosumer")]
        public IActionResult changeDeviceVisibility([FromRoute] long deviceId)
        {
            try
            {
                var credentials = HttpContext.User.Identity as ClaimsIdentity;
                int userId = int.Parse(credentials.FindFirst(ClaimTypes.Actor).Value);

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
