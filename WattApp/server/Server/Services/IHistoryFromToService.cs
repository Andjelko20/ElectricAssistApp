﻿using Microsoft.AspNetCore.Mvc;

namespace Server.Services
{
    public interface IHistoryFromToService
    {
        public double GetCityDoubleHistoryFromTo(string fromDate, string toDate, long deviceCategoryId, long cityId);
        public double GetSettlementDoubleHistoryFromTo(string fromDate, string toDate, long deviceCategoryId, long settlementId);



    }
}
