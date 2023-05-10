﻿using Server.DTOs;
using Server.Models;
using Server.Models.DropDowns.Devices;

namespace Server.Services
{
    public interface IHistoryService
    {
        // ISTORIJA
        //List<DeviceEnergyUsage> GetUsageHistoryForDeviceInLastYear(int deviceId);
        //List<DeviceEnergyUsage> GetUsageHistoryForDeviceInLastMonth(int deviceId);
        //List<DeviceEnergyUsage> GetUsageHistoryForDeviceInLastDay(int deviceId);
        public double GetUsageHistoryForDeviceInLastDay(long deviceId);
        public double GetUsageHistoryForDeviceInLastMonth(long deviceId);
        public double GetUsageHistoryForDeviceInLastYear(long deviceId);
        public double GetUsageHistoryForDeviceInPastWeek(long deviceId);
        public List<MonthlyEnergyConsumptionLastYear> GetMonthlyEnergyUsage(long deviceId);
        public List<DailyEnergyConsumptionPastMonth> GetDailyEnergyUsageForPastMonth(long deviceId);
        public List<DailyEnergyConsumptionPastMonth> GetDailyEnergyUsageForPastWeek(long deviceId);
        public double GetTotalEnergyConsumptionForUser(long userId, long deviceCategoryId);
        public double GetUserEnergyConsumptionForPastDay(long userId, long deviceCategoryId);
        public double GetUserEnergyConsumptionForPastWeek(long userId, long deviceCategoryId);
        public double GetUserEnergyConsumptionForPastMonth(long userId, long deviceCategoryId);
        public double GetUserEnergyConsumptionForPastYear(long userId, long deviceCategoryId);

        // ZA PROSLEDJEN ID KORISNIKA POTROSNJA ZA GRAFIKE
        public List<MonthlyEnergyConsumptionLastYear> GetMonthlyEnergyUsageForPastYear(long userId, long deviceCategoryId);
        public List<DailyEnergyConsumptionPastMonth> UserHistoryForThePastWeek(long userId, long deviceCategoryId);
        public List<EnergyToday> UserHistoryForThePastDayByHour(long userId, long deviceCategoryId);
        public List<DailyEnergyConsumptionPastMonth> UserHistoryForThePastMonth(long userId, long deviceCategoryId);
        public List<DailyEnergyConsumptionPastMonth> SettlementHistoryForThePastWeek(long settlementName, long deviceCategoryName);
        public List<DailyEnergyConsumptionPastMonth> CityHistoryForThePastWeek(long cityId, long deviceCategoryId);
        public List<DailyEnergyConsumptionPastMonth> CityHistoryForThePastMonth(long cityId, long deviceCategoryId);
        public List<DailyEnergyConsumptionPastMonth> SettlementHistoryForThePastMonth(long cityId, long deviceCategoryId);
        public List<MonthlyEnergyConsumptionLastYear> CityHistoryForThePastYearByMonth(long cityId, long deviceCategoryId);
        public List<MonthlyEnergyConsumptionLastYear> SettlementHistoryForThePastYearByMonth(long settlementId, long deviceCategoryId);
        public double GetUsageHistoryForDeviceInThisMonth(long deviceId);
        public double GetUsageHistoryForDeviceToday(long deviceId);
        public double GetUsageHistoryForDeviceThisYear(long deviceId);
        public double GetUsageHistoryForDeviceForPreviousMonth(long deviceId);
        // One year
        public List<MonthlyEnergyConsumptionLastYear> CityHistoryForYearByMonth(long cityId, long deviceCategoryId, int yearNumber);
        public List<MonthlyEnergyConsumptionLastYear> SettlementHistoryForYearByMonth(long settlementId, long deviceCategoryId, int yearNumber);
        public List<MonthlyEnergyConsumptionLastYear> UserHistoryForYearByMonth(long userId, long deviceCategoryId, int yearNumber);
        public List<MonthlyEnergyConsumptionLastYear> DeviceHistoryForYearByMonth(long deviceId, int yearNumber);
        // PAGINACIJA
        public List<DailyEnergyConsumptionPastMonth> GetDailyEnergyUsageForPastMonthPagination(long deviceId, int pageNumber, int itemsPerPage);
        public List<DailyEnergyConsumptionPastMonth> GetProsumerDailyEnergyUsageForPastMonthPagination(long userId, long deviceCategoryId, int pageNumber, int itemsPerPage);
        public List<DailyEnergyConsumptionPastMonth> GetSettlementDailyEnergyUsageForPastMonthPagination(long settlementId, long deviceCategoryId, int pageNumber, int itemsPerPage);
        public List<DailyEnergyConsumptionPastMonth> GetCityDailyEnergyUsageForPastMonthPagination(long cityId, long deviceCategoryId, int pageNumber, int itemsPerPage);

    }
}
