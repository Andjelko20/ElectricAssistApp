using Server.DTOs;
using Server.Models;

namespace Server.Services
{
    public interface IHistoryService
    {
        //List<DeviceEnergyUsage> GetUsageHistoryForDeviceInLastYear(int deviceId);
        //List<DeviceEnergyUsage> GetUsageHistoryForDeviceInLastMonth(int deviceId);
        //List<DeviceEnergyUsage> GetUsageHistoryForDeviceInLastDay(int deviceId);
        public double GetUsageHistoryForDeviceInLastDay(int deviceId);
        public double GetUsageHistoryForDeviceInLastMonth(int deviceId);
        public double GetUsageHistoryForDeviceInLastYear(int deviceId);
        public double GetUsageHistoryForDeviceInPastWeek(int deviceId);
        public List<MonthlyEnergyConsumptionLastYear> GetMonthlyEnergyUsage(int deviceId);
        public List<DailyEnergyConsumptionPastMonth> GetDailyEnergyUsageForPastMonth(int deviceId);
        public List<DailyEnergyConsumptionPastMonth> GetDailyEnergyUsageForPastWeek(int deviceId);
        public double GetTotalEnergyConsumptionForUser(int userId);
    }
}
