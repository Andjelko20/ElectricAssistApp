using Server.DTOs;
using Server.Filters;
using Server.Models;

namespace Server.Services
{
    public interface DeviceService
    {
        /// <summary>
        /// Get device from database by it's unique id if you are DSO
        /// </summary>
        /// <param name="deviceId">Unique device id</param>
        /// <returns>Device object</returns>
        public Device getDeviceById(long deviceId);
        /// <summary>
        /// Get yours device from database by it's unique id if you are PROSUMER
        /// </summary>
        /// <param name="deviceId">Unique device id</param>
        /// <returns>Device object</returns>
        public Device getYourDeviceById(long deviceId, long userId);
        /// <summary>
        /// Get all devices with visibility permission by userId (for DSO)
        /// </summary>
        /// <param name="userId">Unique user id</param>
        /// <returns>List of devices</returns>
        public List<Device> getUserDevices(long userId);
        /// <summary>
        /// Get your devices (for PROSUMER)
        /// </summary>
        /// <param name="userId">Unique user id</param>
        /// <returns>List of devices</returns>
        public DataPage<DeviceResponseDTO> getMyDevices(long userId, DeviceFilterModel filter, int pageNumber, int pageSize);
        /// <summary>
        /// Add new device (for PROSUMER)
        /// </summary>
        /// <param name="device">Device</param>
        /// <returns>Added device</returns>
        public Device addNewDevice(Device device);
        //public List<Device> getAllDevices();
        /// <summary>
        /// Turn on/off your device if you are PROSUMER
        /// </summary>
        /// <param name="deviceId">Id from device you want to turn on/off</param>
        /// <param name="userId">User id to make sure he is changing status of his own device</param>
        /// <returns>Device which status was changed</returns>
        public Device changeTurnOnStatus(long deviceId, long userId);
        /// <summary>
        /// Turn on/off specific device if you are DSO, but only if you have permision
        /// </summary>
        /// <param name="deviceId">Id from device you want to turn on/off</param>
        /// <returns>Device which status was changed</returns>
        public Device changeTurnOnStatus(long deviceId);

        public Device editDevice(Device device, long userId);
        /// <summary>
        /// Delete device by its unique id (for PROSUMER)
        /// </summary>
        /// <param name="id">Device id</param>
        /// <returns>Deleted device</returns>
        public Device deleteDeviceById(long id, long userId);
        //public List<Device> getUsersDevices(long userId);
        /// <summary>
        /// Change the visibility status for the device by its unique id (for PROSUMER)
        /// </summary>
        /// <param name="deviceId">Id for device</param>
        /// <param name="userId">Id for user</param>
        /// <returns>Changed device</returns>
        public Device changeDeviceVisibility(long deviceId, long userId);
        /// <summary>
        /// Change the controlability status for the device by its unique id (for PROSUMER)
        /// </summary>
        /// <param name="deviceId">Id for device</param>
        /// <param name="userId">Id for user (logged user)</param>
        /// <returns></returns>
        public Device changeDeviceControlability(long deviceId, long userId);
    }
}
