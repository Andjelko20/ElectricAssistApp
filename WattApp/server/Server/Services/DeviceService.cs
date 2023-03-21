using Server.Models;

namespace Server.Services
{
    public interface DeviceService
    {
        public Device getDeviceById(long deviceId);
        public Device addNewDevice(Device device);
        public List<Device> getAllDevices();
        public Device changeTurnOnStatus(long deviceId);
        public Device editDevice(Device device);
        public Device deleteDeviceById(long id);

    }
}
