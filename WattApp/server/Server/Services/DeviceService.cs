using Server.DTOs;
using Server.Models;

namespace Server.Services
{
    public interface DeviceService
    {
        public Device getDeviceById(long deviceId);
        public Device addNewDevice(Device device);
        public List<Device> getAllDevices(long roleId);
        public Device changeTurnOnStatus(long deviceId, UserCheckDTO userCheck);
        public Device editDevice(Device device);
        public Device deleteDeviceById(long id);
        public List<Device> getAllUsersDevices(long userId, long roleId);
    }
}
