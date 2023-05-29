using Server.DTOs;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;

namespace Server.Services
{
    public interface IProsumerService
    {
        public double GetTotalConsumptionInTheMomentForCity(long deviceCategoryId, long cityId);
        public double GetTotalConsumptionInTheMomentForOneProsumer(long deviceCategoryId, long userId);
        public double GetTotalConsumptionInTheMomentForSettlement(long deviceCategoryId, long settlementId);
        public double GetAverageConsumptionInTheMomentForCity(long settlementName, double totalEnergyUsage);
        public int GetNumberOfDevicesOfOneProsumer(long userId);
        public DeviceTimeDTO FromWhenToWhenDeviceWorks(long deviceId, DateTime turnedOn, DateTime turnedOff);
    }
}
