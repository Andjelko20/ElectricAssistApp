﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Models.DropDowns.Devices;
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
        /*[HttpGet]
        public string getCategoryNameById(long categoryId)
        {
            DeviceCategory category = _service.getCategoryNameById(categoryId);
            return category.Name;
        }*/

        [HttpGet("categories")]
        public List<DeviceCategoryDTO> getAllCategories()
        {
            return _mapper.Map<List<DeviceCategoryDTO>>(_service.getAllCategories());
        }
    }
}
