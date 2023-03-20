using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceBrandController : ControllerBase
    {
        DeviceBrandService _service;
        public DeviceBrandController(DeviceBrandService service)
        {
            _service = service;
        }

        [HttpGet]
        public string getBrandNameById(long brandId)
        {
            return _service.getBrandNameById(brandId);
        }
    }
}
