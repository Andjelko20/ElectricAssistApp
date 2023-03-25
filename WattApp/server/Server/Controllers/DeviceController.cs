using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Exceptions;
using Server.Models;
using Server.Services;
using System.Net;
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
            try {
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
                if(deviceDTO == null)
                {
                    throw new ItemNotFoundException("Device with id: " + id + " not found!");
                }

                deviceDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(deviceDTO.DeviceCategory));
                deviceDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(deviceDTO.DeviceType));
                deviceDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(deviceDTO.DeviceBrand));
                deviceDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(deviceDTO.DeviceModel));

                return Ok(deviceDTO);
            }
            catch(ItemNotFoundException ex)
            {
                return NotFound(new
                {
                    title =  "Device not found!", 
                    status = 404,
                    message = ex.Message,
                });
            }
            catch(Exception ex)
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
        public DeviceResponseDTO addNewDevice([FromBody]DeviceRequestDTO deviceRequestDTO)
        {
            DeviceResponseDTO deviceDTO = _mapper.Map<DeviceResponseDTO>(_deviceService.addNewDevice(_mapper.Map<Device>(deviceRequestDTO)));

            deviceDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(deviceDTO.DeviceCategory));
            deviceDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(deviceDTO.DeviceType));
            deviceDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(deviceDTO.DeviceBrand));
            deviceDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(deviceDTO.DeviceModel));

            return deviceDTO;
        }

        /// <summary>
        /// Get all visible devices for every user if you are DSO and only your devices if you are PROSUMER.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "dispecer, prosumer")]
        public List<DeviceResponseDTO> getAllDevices()
        {
            List<Device> devices = new List<Device>();
            List<DeviceResponseDTO> responseDTOs = new List<DeviceResponseDTO>();
            if (User.IsInRole("prosumer"))
            {
                var credentials = HttpContext.User.Identity as ClaimsIdentity;
                int userId = int.Parse(credentials.FindFirst(ClaimTypes.Actor).Value);

                devices = _deviceService.getUsersDevices(userId);
            }
            else
            {
                devices = _deviceService.getAllDevices();
            }

            foreach(Device device in devices)
            {
                DeviceResponseDTO responseDTO = _mapper.Map<DeviceResponseDTO>(device);

                responseDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(responseDTO.DeviceCategory));
                responseDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(responseDTO.DeviceType));
                responseDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(responseDTO.DeviceBrand));
                responseDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(responseDTO.DeviceModel));

                responseDTOs.Add(responseDTO);
            }

            return responseDTOs;
        }

        [HttpPut("turnOn")]
        [Authorize(Roles = "dispecer, prosumer")]
        public DeviceResponseDTO changeTurnOnStatus(long deviceId)
        {
            Device device = new Device();
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
            

            return _mapper.Map<DeviceResponseDTO>(device);
        }



        [HttpPut]
        [Authorize(Roles = "prosumer")]
        public DeviceResponseDTO editDevice([FromBody]DeviceRequestDTO deviceRequestDTO)
        {
            DeviceResponseDTO deviceDTO = _mapper.Map<DeviceResponseDTO>(_deviceService.editDevice(_mapper.Map<Device>(deviceRequestDTO)));

            deviceDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(deviceDTO.DeviceCategory));
            deviceDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(deviceDTO.DeviceType));
            deviceDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(deviceDTO.DeviceBrand));
            deviceDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(deviceDTO.DeviceModel));

            return deviceDTO;
        }

        [HttpDelete("{id:long}")]
        public DeviceResponseDTO deleteDeviceById([FromRoute]long id)
        {
           return  _mapper.Map<DeviceResponseDTO>(_deviceService.deleteDeviceById(id));
        }

        [HttpPut("controlability")]
        [Authorize(Roles = "prosumer")]
        public DeviceResponseDTO changeDeviceControlability(long deviceId)
        {
            var credentials = HttpContext.User.Identity as ClaimsIdentity;
            int userId = int.Parse(credentials.FindFirst(ClaimTypes.Actor).Value);

            DeviceResponseDTO deviceDTO = _mapper.Map<DeviceResponseDTO>(_deviceService.changeDeviceControlability(deviceId, userId));

            deviceDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(deviceDTO.DeviceCategory));
            deviceDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(deviceDTO.DeviceType));
            deviceDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(deviceDTO.DeviceBrand));
            deviceDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(deviceDTO.DeviceModel));

            return deviceDTO;
        }

        [HttpPut("visibility")]
        [Authorize(Roles = "prosumer")]
        public DeviceResponseDTO changeDeviceVisibility(long deviceId)
        {
            var credentials = HttpContext.User.Identity as ClaimsIdentity;
            int userId = int.Parse(credentials.FindFirst(ClaimTypes.Actor).Value);

            DeviceResponseDTO deviceDTO = _mapper.Map<DeviceResponseDTO>(_deviceService.changeDeviceVisibility(deviceId, userId));

            deviceDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(deviceDTO.DeviceCategory));
            deviceDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(deviceDTO.DeviceType));
            deviceDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(deviceDTO.DeviceBrand));
            deviceDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(deviceDTO.DeviceModel));

            return deviceDTO;
        }
    }
}
