using Server.Data;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Devices.Agregations;
using Server.Models.DropDowns.Location;

namespace Server.Services.Implementations
{

    public class DropDownServiceImpl : DropDownService
    {
        private readonly SqliteDbContext _db;
        private readonly DeviceBrandService _deviceBrandService;
        private readonly DeviceModelService _deviceModelService;
        public DropDownServiceImpl(SqliteDbContext db, DeviceBrandService deviceBrandService, DeviceModelService deviceModelService)
        {
            _db = db;
            _deviceBrandService = deviceBrandService;
            _deviceModelService = deviceModelService;
        }

        public List<Settlement> getSettlements(long cityId)
        {
            return _db.Settlements.Where(t => t.CityId == cityId).ToList();
        }

        public List<City> getCities(long countryId)
        {
            return _db.Cities.Where(c => c.CountryId == countryId).ToList();
        }

        public List<Country> getCountries()
        {
            return _db.Countries.ToList();
        }

        public List<DeviceBrand> getDeviceBrand(long deviceTypeId)
        {

            //List<TypeBrand> brands = _db.TypeBrands.Where(x => x.TypeId == deviceTypeId).ToList();
            List<DeviceBrand> devices = new List<DeviceBrand>();
            //foreach(TypeBrand brand in brands)
            //{
            //    DeviceBrand deviceBrand = _deviceBrandService.getBrandById(brand.BrandId);
            //    devices.Add(deviceBrand);
            //}
            return devices;
        }

        public List<DeviceCategory> getDeviceCategories()
        {
            return _db.DeviceCategories.ToList();
        }

        public List<DeviceModel> getDeviceModel(long deviceTypeId, long deviceBrandId)
        {
            //List<TypeBrandModel> models = _db.TypeBrandModels.Where(x => x.TypeId == deviceTypeId && x.BrandId == deviceBrandId).ToList();
            List<DeviceModel> deviceModels = new List<DeviceModel>();
            //foreach(TypeBrandModel model in models)
            //{
            //    DeviceModel deviceModel = _deviceModelService.getDeviceModel(model.ModelId);
            //    deviceModels.Add(deviceModel);
            //}
            return deviceModels;
        }

        public List<DeviceType> getDeviceTypes(long deviceCategoryId)
        {
            return _db.DeviceTypes.Where(x => x.CategoryId == deviceCategoryId).ToList();
        }

        
    }
}
