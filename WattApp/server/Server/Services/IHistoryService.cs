﻿using Server.Models;

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
    }
}
