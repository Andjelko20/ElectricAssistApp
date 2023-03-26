using Server.Models;

namespace Server.Services
{
    public interface IHistoryService
    {
        List<DeviceEnergyUsage> GetUsageHistoryForDeviceInLastYear(int deviceId);
        List<DeviceEnergyUsage> GetUsageHistoryForDeviceInLastMonth(int deviceId);
    }
}
