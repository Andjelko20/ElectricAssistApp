using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using System.Globalization;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceCategoryController : ControllerBase
    {
        DeviceCategoryService _service;
        public DeviceCategoryController(DeviceCategoryService service)
        {
            _service = service;
        }
        [HttpGet]
        public string getCategoryNameById(long categoryId)
        {
            return _service.getCategoryNameById(categoryId);
        }
    }
}
