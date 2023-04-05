using Server.Models;

namespace Server.Filters
{
    public class DeviceFilter
    {
        public static IQueryable<Device> ApplyFilter(IQueryable<Device> devices, DeviceFilterModel filter)
        {

            if (filter == null)
            {
                return devices;
            }
            if (filter.categoryId != null)
            {
                devices = devices.Where(src => src.DeviceModel.DeviceType.CategoryId == filter.categoryId);
            }
            if (filter.typeId != null)
            {
                devices = devices.Where(src => src.DeviceModel.DeviceTypeId == filter.typeId);
            }
            if (filter.brandId != null)
            {
                devices = devices.Where(src => src.DeviceModel.DeviceBrandId == filter.brandId);
            }
            if (filter.turnOn != null)
            {
                devices = devices.Where(src => src.TurnOn == filter.turnOn);
            }
            if (filter.controlability != null)
            {
                devices = devices.Where(src => src.Controlability == filter.controlability);
            }

            return devices;
        }
    }
}
