using Server.DTOs;
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

        public List<Device> getUserDevices(long userId);
        public List<Device> getMyDevices(long userId);
        /// <summary>
        /// Dodaje nov uredjaj u bazu
        /// </summary>
        /// <param name="device">Uredjaj koji se dodaje</param>
        /// <returns>Dodat uredjaj</returns>
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
        public Device editDevice(Device device);
        public Device deleteDeviceById(long id);
        //public List<Device> getUsersDevices(long userId);
        public Device changeDeviceVisibility(long deviceId, long userId);
        public Device changeDeviceControlability(long deviceId, long userId);
    }
}
