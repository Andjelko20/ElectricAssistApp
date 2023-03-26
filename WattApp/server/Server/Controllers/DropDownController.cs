using Microsoft.AspNetCore.Mvc;
using Server.Models.DropDowns.Devices;
using Server.Services;

namespace Server.Controllers
{
    public class DropDownController
    {
        private readonly DropDownService _service;
        public DropDownController(DropDownService service)
        {
            _service = service;
        }

        [HttpGet("categories")]
        public List<DeviceCategory> getCategories()
        {
            return _service.getDeviceCategories();
        }

        [HttpGet("types")]
        public List<DeviceType> getDeviceTypes(long categoryId)
        {
            return _service.getDeviceTypes(categoryId);
        }

        [HttpGet("brands")]
        public List<DeviceBrand> getDeviceBrands(long typeId)
        {
            return _service.getDeviceBrand(typeId);
        }

        [HttpGet("models")]
        public List<DeviceModel> getDeviceModels(long typeId, long brandId)
        {
            return _service.getDeviceModel(typeId, brandId);
        }



    }
}
