using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/devices")]
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

        [HttpPost]
        public DeviceResponseDTO addNewDevice(DeviceRequestDTO deviceRequestDTO)
        {
            DeviceResponseDTO deviceDTO = _mapper.Map<DeviceResponseDTO>(_deviceService.addNewDevice(_mapper.Map<Device>(deviceRequestDTO)));

            deviceDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(deviceDTO.DeviceCategory));
            deviceDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(deviceDTO.DeviceType));
            deviceDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(deviceDTO.DeviceBrand));
            deviceDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(deviceDTO.DeviceModel));

            return deviceDTO;
        }

        [HttpGet("/allDevices")]
        public List<DeviceResponseDTO> getAllDevices()
        {
            List<Device> devices = _deviceService.getAllDevices();
            List<DeviceResponseDTO> responses = new List<DeviceResponseDTO>();
            foreach (Device device in devices)
            {
                DeviceResponseDTO deviceDTO = _mapper.Map<DeviceResponseDTO>(device);

                deviceDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(deviceDTO.DeviceCategory));
                deviceDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(deviceDTO.DeviceType));
                deviceDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(deviceDTO.DeviceBrand));
                deviceDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(deviceDTO.DeviceModel));

                responses.Add(deviceDTO);
            }
            return responses;
        }

        [HttpPut("/turnOn")]
        public DeviceResponseDTO changeTurnOnStatus(long id)
        {
            return _mapper.Map<DeviceResponseDTO>(_deviceService.changeTurnOnStatus(id));
        }

        [HttpPut]
        public DeviceResponseDTO editDevice(DeviceRequestDTO deviceRequestDTO)
        {
            DeviceResponseDTO deviceDTO = _mapper.Map<DeviceResponseDTO>(_deviceService.editDevice(_mapper.Map<Device>(deviceRequestDTO)));

            deviceDTO.DeviceCategory = _deviceCategoryService.getCategoryNameById(long.Parse(deviceDTO.DeviceCategory));
            deviceDTO.DeviceType = _deviceTypeService.getTypeNameById(long.Parse(deviceDTO.DeviceType));
            deviceDTO.DeviceBrand = _deviceBrandService.getBrandNameById(long.Parse(deviceDTO.DeviceBrand));
            deviceDTO.DeviceModel = _deviceModelService.getModelNameById(long.Parse(deviceDTO.DeviceModel));

            return deviceDTO;
        }

        [HttpDelete]
        public DeviceResponseDTO deleteDeviceById(long id)
        {
           return  _mapper.Map<DeviceResponseDTO>(_deviceService.deleteDeviceById(id));
        }
    }
}
