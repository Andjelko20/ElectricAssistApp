using Server.DTOs;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;

namespace Server.Services
{
    public interface IProsumerService
    {
        public double GetTotalConsumptionInTheMoment(long deviceCategoryName);
        public double GetTotalNumberOfDevicesInTheCity(long deviceCategoryId, long cityId);
        public double GetTotalNumberOfDevicesInTheSettlement(long deviceCategoryId, long cityId, long settlementId);
        public double GetTotalConsumptionInTheMomentForSettlement(long deviceCategoryId, long settlementId);
        public double GetTotalConsumptionInTheMomentForCity(long deviceCategoryId, long cityId);
        public double GetTotalConsumptionInTheMomentForOneProsumer(long deviceCategoryId, long userId);
        public double GetAverageConsumptionInTheMomentForSettlement(long settlementId, double totalEnergyUsage);
        public double GetAverageConsumptionInTheMomentForCity(long settlementName, double totalEnergyUsage);
        public double GetAverageConsumptionProductionInTheMomentForAllProsumers(double totalEnergyUsage);
        public List<EnergyToday> CalculateEnergyUsageForToday(long deviceId);
    }
}
