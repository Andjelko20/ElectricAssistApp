using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Services;
using System.Globalization;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceCategoryController : ControllerBase
    {
        DeviceCategoryService _service;
        IMapper _mapper;
        public DeviceCategoryController(DeviceCategoryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        [HttpGet]
        public string getCategoryNameById(long categoryId)
        {
            return _service.getCategoryNameById(categoryId);
        }

        [HttpGet("/categories")]
        public List<DeviceCategoryDTO> getAllCategories()
        {
            return _mapper.Map<List<DeviceCategoryDTO>>(_service.getAllCategories());
        }
    }
}
