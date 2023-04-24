using Server.Data;
using Server.Models;

namespace Server.Services.Implementations
{
    public class CurrentPeriodHistoryImpl : ICurrentPeriodHistoryService
    {
        private readonly SqliteDbContext _context;
        public CurrentPeriodHistoryImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public double GetUsageHistoryForDeviceFromCurrentYear(long deviceId)
        {
            DateTime startOfTheYear = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
            DateTime theTime = DateTime.Now;
            double consumption = 0.0;

            var deviceModelEnergyKwh = _context.Devices
                .Where(d => d.Id == deviceId)
                .Select(d => d.DeviceModel.EnergyKwh)
                .FirstOrDefault();

            var energyUsages = _context.DeviceEnergyUsages
                .Where(du => du.DeviceId == deviceId && du.StartTime >= startOfTheYear && (du.EndTime <= theTime || du.EndTime == null))
                .ToList();

            foreach (var energyUsage in energyUsages)
            {
                var hours = Math.Abs((energyUsage.EndTime - energyUsage.StartTime).TotalHours);
                consumption += (double)(deviceModelEnergyKwh * hours);
            }

            return Math.Round(consumption, 2);
        }

        public double GetUsageHistoryForDeviceFromCurrentMonth(long deviceId)
        {
            DateTime startOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            DateTime TheTime = DateTime.Now;

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= startOfTheMonth && (u.EndTime <= TheTime || u.EndTime == null))
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }

        public double GetConsumptionForForwardedList(long deviceId, List<DeviceEnergyUsage> deviceEnergyUsageLista)
        {
            var Device = _context.Devices.Where(u => u.Id == deviceId).FirstOrDefault();
            var DeviceModel = _context.DeviceModels.FirstOrDefault(dm => dm.Id == Device.DeviceModelId);
            float EnergyInKwh = DeviceModel.EnergyKwh;

            double Consumption = 0.0;
            double Hours = -1;
            foreach (var item in deviceEnergyUsageLista)
            {
                Hours = Math.Abs((item.EndTime - item.StartTime).TotalHours);
                Consumption += (double)(EnergyInKwh * Hours);
            }

            return Math.Round(Consumption, 2);
        }

        public double GetUsageHistoryForDeviceFromCurrentDay(long deviceId)
        {
            DateTime startOfTheDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime TheTime = DateTime.Now;

            List<DeviceEnergyUsage> deviceEnergyUsageLista = _context.DeviceEnergyUsages
                                                            .Where(u => u.DeviceId == deviceId && u.StartTime >= startOfTheDay && u.EndTime <= TheTime)
                                                            .OrderBy(u => u.StartTime)
                                                            .ToList();

            return GetConsumptionForForwardedList(deviceId, deviceEnergyUsageLista);
        }
    }
}
