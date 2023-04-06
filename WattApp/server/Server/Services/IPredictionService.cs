﻿using Server.DTOs;

namespace Server.Services
{
    public interface IPredictionService
    {
        public List<DailyEnergyConsumptionPastMonth> ConsumptionPredictionForTheNextWeek(long deviceId);
        public List<DailyEnergyConsumptionPastMonth> UserPredictionForTheNextWeek(long userId, long deviceCategoryId);
    }
}