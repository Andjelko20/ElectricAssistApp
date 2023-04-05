using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;

namespace Server.Services
{
    public interface DropDownService
    {
        public List<DeviceCategory> getDeviceCategories();
        public List<DeviceType> getDeviceTypes(long deviceCategoryId);
        public List<DeviceBrand> getDeviceBrand(long deviceTypeId);
        public List<DeviceModel> getDeviceModel(long deviceTypeId, long deviceBrandId);
        public List<Country> getCountries();
        public List<City> getCities(long countryId);
        public List<Settlement> getSettlements(long cityId);
    }
}
