using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;
using Server.Services;
using Server.Services.Implementations;

namespace Server.Controllers
{
    [ApiController]
    public class DropDownController:ControllerBase
    {
        private readonly DropDownService _service;
        private readonly ITokenService tokenService;
        private readonly IUserService userService;
        public DropDownController(DropDownService service,ITokenService tokenService,IUserService userService)
        {
            _service = service;
            this.tokenService = tokenService;
            this.userService = userService;
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

        [HttpGet("settlements")]
        public List<Settlement> getSettlements(long cityId)
        {
            return _service.getSettlements(cityId);
        }

        [HttpGet("my_settlements")]
        [Authorize]
        public async Task<List<Settlement>> GetMySettlementsAsync()
        {
            var id = long.Parse(tokenService.GetClaim(this.HttpContext, "Id"));
            var user = await userService.GetUserById(id);
            long cityId = user.Settlement.CityId;
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
