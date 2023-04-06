using Server.Models.DropDowns.Devices;

namespace Server.Services
{
    public interface DeviceTypeService
    {
        public string getTypeNameById(long typeId);
        public List<DeviceType> GetDeviceTypesByCategory(long categoryId);
    }
}
