using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Mappers;
using Server.Models;
using Server.Services;
using System.Globalization;
using System.Numerics;

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

        [HttpGet]
        public DeviceResponseDTO getElementById(long id)
        {
            DeviceResponseDTO deviceDTO = _mapper.Map<DeviceResponseDTO>(_deviceService.getDeviceById(id));

            deviceDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(deviceDTO.DeviceCategory));
            deviceDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(deviceDTO.DeviceType));
            deviceDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(deviceDTO.DeviceBrand));
            deviceDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(deviceDTO.DeviceModel));

            return deviceDTO;
        }
    }
}
