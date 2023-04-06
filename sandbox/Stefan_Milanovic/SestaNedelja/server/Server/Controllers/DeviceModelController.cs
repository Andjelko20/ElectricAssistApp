using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceModelController : ControllerBase
    {
        DeviceModelService _service;
        public DeviceModelController(DeviceModelService service)
        {
            _service = service;
        }
        /*[HttpGet]
        public string getModelNameById(long modelId)
        {
            return _service.getModelNameById(modelId);
        }*/
    }
}
