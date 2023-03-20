using Server.Models;

namespace Server.Services
{
    public interface DeviceService
    {
        public Device getDeviceById(long deviceId);
        public Device addNewDevice(Device device);

    }
}
