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
