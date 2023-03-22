using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models.DropDowns.Devices;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceTypeController : ControllerBase
    {
        DeviceTypeService _service;
        public DeviceTypeController(DeviceTypeService service)
        {
            _service = service;
        }
        [HttpGet]
        public string getTypeNameById(long typeId)
        {
            return _service.getTypeNameById(typeId);
        }
        [HttpGet("/types")]
        public List<DeviceType> GetDeviceTypesByCategory(long categoryId)
        {
            return _service.GetDeviceTypesByCategory(categoryId);
        }
    }
}
