﻿using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.DTOs;
using Server.Models.DropDowns.Devices;
using Server.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryFromToController : Controller
    {
        public readonly SqliteDbContext _sqliteDb;
        public readonly IHistoryFromToService historyFromToService;

        public HistoryFromToController(SqliteDbContext sqliteDb, IHistoryFromToService historyFromToService)
        {
            _sqliteDb = sqliteDb;
            this.historyFromToService = historyFromToService;
        }

        /// <summary>
        /// Histroy - From To
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetHistoryForCity([FromQuery] string fromDate, string toDate, 
                                                                       long deviceCategoryId,
                                                                       long cityId, long settlementId, 
                                                                       long byMonthCityId, long byDayCityId, long byHourCityId,
                                                                       long byMonthSettlementId, long byDaySettlementId, long byHourSettlementId, 
                                                                       long byMonthUserId, long byDayUserId, long byHourUserId, long doubleUserId,
                                                                       long doubleDeviceId, long byMonthDeviceId, long byHourDeviceId, long byDayDeviceId)
        {
            if(cityId!=0)
            {
                if (!_sqliteDb.Cities.Any(c => c.Id == cityId))
                    return NotFound(new { message = "City with the ID: " + cityId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                double result = historyFromToService.GetCityDoubleHistoryFromTo(fromDate, toDate, deviceCategoryId, cityId);
                return Ok(result);
            }
            else if(settlementId!=0)
            {
                if (!_sqliteDb.Settlements.Any(s => s.Id == settlementId))
                    return NotFound(new { message = "Settlement with the ID: " + settlementId.ToString() + " does not exist." });
                if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                double result = historyFromToService.GetSettlementDoubleHistoryFromTo(fromDate, toDate, deviceCategoryId, settlementId);
                return Ok(result);
            }
            else if (byDayCityId != 0)
            {
                if (!_sqliteDb.Cities.Any(c => c.Id == byDayCityId))
                    return NotFound(new { message = "City with the ID: " + byDayCityId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                List<DailyEnergyConsumptionPastMonth> result = historyFromToService.GetCityHistoryByDayFromTo(fromDate, toDate, deviceCategoryId, byDayCityId);
                return Ok(result);
            }
            else if (byDaySettlementId != 0)
            {
                if (!_sqliteDb.Settlements.Any(s => s.Id == byDaySettlementId))
                    return NotFound(new { message = "Settlement with the ID: " + byDaySettlementId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                List<DailyEnergyConsumptionPastMonth> result = historyFromToService.GetSettlementHistoryByDayFromTo(fromDate, toDate, deviceCategoryId, byDaySettlementId);
                return Ok(result);
            }
            else if (byHourCityId != 0)
            {
                if (!_sqliteDb.Settlements.Any(s => s.Id == byHourCityId))
                    return NotFound(new { message = "Settlement with the ID: " + byHourCityId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                List<EnergyToday> result = historyFromToService.GetCityHistoryByHourFromTo(fromDate, toDate, deviceCategoryId, byHourCityId);
                return Ok(result);
            }
            else if (byHourSettlementId != 0)
            {
                if (!_sqliteDb.Settlements.Any(s => s.Id == byHourSettlementId))
                    return NotFound(new { message = "Settlement with the ID: " + byHourSettlementId.ToString() + " does not exist." });

                if (!_sqliteDb.DeviceCategories.Any(dc => dc.Id == deviceCategoryId))
                    return NotFound(new { message = "Device category with the ID " + deviceCategoryId.ToString() + " does not exist." });

                List<EnergyToday> result = historyFromToService.GetSettlementHistoryByHourFromTo(fromDate, toDate, deviceCategoryId, byHourSettlementId);
                return Ok(result);
            }
            else if(byHourUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == byHourUserId))
                    return NotFound(new { message = "User with ID: " + byHourUserId.ToString() + " does not exist." });
                
                List<EnergyToday> result = historyFromToService.GetProsumerHistoryByHourFromTo(fromDate, toDate, byHourUserId, deviceCategoryId);
                return Ok(result);
            }
            else  if(byDayUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == byDayUserId))
                    return NotFound(new { message = "User with ID: " + byDayUserId.ToString() + " does not exist." });

                var result = historyFromToService.GetProsumerHistoryByDayFromTo(fromDate, toDate, byDayUserId, deviceCategoryId);
                return Ok(result);
            }
            else if(doubleUserId != 0)
            {
                if (!_sqliteDb.Users.Any(u => u.Id == doubleUserId))
                    return NotFound(new { message = "User with ID: " + doubleUserId.ToString() + " does not exist." });

                var result = historyFromToService.GetProsumerDoubleHistoryFromTo(fromDate, toDate, doubleUserId, deviceCategoryId);
                return Ok(result);
            }
            else if (doubleDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(d => d.Id == doubleDeviceId))
                    return NotFound(new { message = "Device with ID: " + doubleDeviceId.ToString() + " does not exist." });

                var result = historyFromToService.GetDeviceDoubleHistoryFromTo(fromDate, toDate, doubleDeviceId);
                return Ok(result);
            }
            else if (byDayDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(d => d.Id == byDayDeviceId))
                    return NotFound(new { message = "Device with ID: " + byDayDeviceId.ToString() + " does not exist." });

                var result = historyFromToService.GetDeviceHistoryByDayFromTo(fromDate, toDate, byDayDeviceId);
                return Ok(result);
            }
            else if (byHourDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(d => d.Id == byHourDeviceId))
                    return NotFound(new { message = "Device with ID: " + byHourDeviceId.ToString() + " does not exist." });

                var result = historyFromToService.GetDeviceHistoryByHourFromTo(fromDate, toDate, byHourDeviceId);
                return Ok(result);
            }
            else if (byMonthDeviceId != 0)
            {
                if (!_sqliteDb.Devices.Any(d => d.Id == byMonthDeviceId))
                    return NotFound(new { message = "Device with ID: " + byMonthDeviceId.ToString() + " does not exist." });

                var result = historyFromToService.GetDeviceHistoryByMonthFromTo(fromDate, toDate, byMonthDeviceId);
                return Ok(result);
            }
            else if (byMonthUserId != 0)
            {
                if (!_sqliteDb.Users.Any(d => d.Id == byMonthUserId))
                    return NotFound(new { message = "User with ID: " + byMonthUserId.ToString() + " does not exist." });

                var result = historyFromToService.GetProsumerHistoryByMonthFromTo(fromDate, toDate, byMonthUserId, deviceCategoryId);
                return Ok(result);
            }
            else if (byMonthCityId != 0)
            {
                if (!_sqliteDb.Cities.Any(c => c.Id == byMonthCityId))
                    return NotFound(new { message = "City with ID: " + byMonthCityId.ToString() + " does not exist." });

                var result = historyFromToService.GetCityHistoryByMonthFromTo(fromDate, toDate, deviceCategoryId, byMonthCityId);
                return Ok(result);
            }
            else // if (byMonthSettlementId != 0)
            {
                if (!_sqliteDb.Settlements.Any(s => s.Id == byMonthSettlementId))
                    return NotFound(new { message = "Settlement with ID: " + byMonthSettlementId.ToString() + " does not exist." });

                var result = historyFromToService.GetSettlementHistoryByMonthFromTo(fromDate, toDate, deviceCategoryId, byMonthSettlementId);
                return Ok(result);
            }
        }

        /// <summary>
        /// Histroy - From To
        /// </summary>
        [HttpGet]
        [Route("Pagination")]
        public async Task<IActionResult> GetHistoryFromDateToDatePagination([FromQuery] int pageNumber, int itemsPerPage, string fromDate, string toDate,
                                                                                        long byHourDeviceId, long deviceCategoryId, long byHourUserId,
                                                                                        long byHourSettlementId, long byHourCityId)
        {
            if (pageNumber > 0 && itemsPerPage > 0)
            {
                if (byHourDeviceId != 0)
                {
                    if (!_sqliteDb.Devices.Any(d => d.Id == byHourDeviceId))
                        return NotFound(new { message = "Device with ID: " + byHourDeviceId.ToString() + " does not exist." });

                    var result = historyFromToService.GetDeviceHistoryByHourFromToPagination(fromDate, toDate, byHourDeviceId, pageNumber, itemsPerPage);
                    return Ok(result);
                }
                else if (byHourUserId != 0)
                {
                    if (!_sqliteDb.Users.Any(u => u.Id == byHourUserId))
                        return NotFound(new { message = "User with ID: " + byHourUserId.ToString() + " does not exist." });

                    var result = historyFromToService.GetUserHistoryByHourFromToPagination(fromDate, toDate, byHourUserId, deviceCategoryId, pageNumber, itemsPerPage);
                    return Ok(result);
                }
                else if (byHourSettlementId != 0)
                {
                    if (!_sqliteDb.Settlements.Any(s => s.Id == byHourSettlementId))
                        return NotFound(new { message = "Settlement with ID: " + byHourSettlementId.ToString() + " does not exist." });

                    var result = historyFromToService.GetSettlementHistoryByHourFromToPagination(fromDate, toDate, byHourSettlementId, deviceCategoryId, pageNumber, itemsPerPage);
                    return Ok(result);
                }
                else //if (byHourCityId != 0)
                {
                    if (!_sqliteDb.Cities.Any(c => c.Id == byHourCityId))
                        return NotFound(new { message = "City with ID: " + byHourCityId.ToString() + " does not exist." });

                    var result = historyFromToService.GetCityHistoryByHourFromToPagination(fromDate, toDate, byHourCityId, deviceCategoryId, pageNumber, itemsPerPage);
                    return Ok(result);
                }
            }
            else
            {
                return NotFound(new { message = "Value for pageNumber and itemsPerPage must be greater then 0."});
            }
        }
    }
}