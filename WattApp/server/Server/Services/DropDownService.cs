using Server.Models.DropDowns.Devices;

namespace Server.Services
{
    public interface DropDownService
    {
        public List<DeviceCategory> getDeviceCategories();
        public List<DeviceType> getDeviceTypes(long deviceCategoryId);
        public List<DeviceBrand> getDeviceBrand(long deviceTypeId);
        public List<DeviceModel> getDeviceModel(long deviceTypeId, long deviceBrandId);
    }
}
