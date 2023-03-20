using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Mappers;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        DeviceService _deviceService;
        IMapper _mapper;
        public DeviceController(DeviceService deviceService, IMapper mapper)
        {
            _deviceService = deviceService;
            _mapper = mapper;
        }

        [HttpGet]
        public DeviceResponseDTO getElementById(long id)
        {
            Device device = _deviceService.getDeviceById(id);
            return _mapper.Map<DeviceResponseDTO>(device);
        }
    }
}
