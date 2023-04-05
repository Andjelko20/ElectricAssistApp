using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;

namespace Server.Services
{
    public interface IProsumerService
    {
        public double GetTotalConsumptionInTheMoment(string deviceCategoryName);
        public double GetTotalNumberOfDevicesInTheCity(long deviceCategoryId, long cityId);
        public double GetTotalNumberOfDevicesInTheSettlement(long deviceCategoryId, long cityId, long settlementId);
        public double GetTotalConsumptionInTheMomentForSettlement(string deviceCategoryName, string settlementName);
    }
}
