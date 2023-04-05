using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;
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

        /*[HttpGet("categories")]
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
        }*/

        [HttpGet("settlements")]
        public List<Settlement> getSettlements(long cityId)
        {
            return _service.getSettlements(cityId);
        }
        [HttpGet("cities")]
        public List<City> getCities(long countryId)
        {
            return _service.getCities(countryId);
        }
        [HttpGet("countries")]
        public List<Country> getCountries()
        {
            return _service.getCountries();
        }





    }
}
