using Server.Models.DropDowns.Devices;

namespace Server.Services
{
    public interface DeviceCategoryService
    {
        public string getCategoryNameById(long categoryId);
        public List<DeviceCategory> getAllCategories();
    }
}
