using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models;
using System.Linq;

namespace Server.Services.Implementations
{
    public class ProsumerServiceImpl : IProsumerService
    {
        private readonly SqliteDbContext _context;
        public ProsumerServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        /*GetTotalConsumptionInTheMoment
        {
            double TotalEnergyUsage = -100.0;
            var Devices = _context.Devices.Where(d => _context.DeviceModels
                                                      .Where(dm => _context.DeviceTypes
                                                                   .Where(dt => dt.CategoryId == 1)
                                                                   .Select(dt => dt.Id)
                                                                   .Contains(dm.DeviceTypeId))
                                                      .Select(dm => dm.Id)
                                                      .Contains(d.DeviceModelId))
                                           .ToList();

            foreach (var Device in Devices)
            {
                var DeviceUsages = _context.DeviceEnergyUsages
                    .Where(u => u.DeviceId == Device.Id && u.StartTime <= DateTime.Now.AddHours(-1) && u.EndTime <= DateTime.Now)
                    .ToList();
                if(DeviceUsages == null)
                    Console.WriteLine("*****************NULL"+DeviceUsages.Count);
                else
                    Console.WriteLine("***************NOTNULL" + DeviceUsages.Count);

                foreach (var usage in DeviceUsages)
                {
                    Console.WriteLine("***************sadadsadasd " + usage.DeviceId + " " + usage.StartTime + " " + usage.EndTime);
                    TotalEnergyUsage += (usage.EndTime - usage.StartTime).TotalHours * Device.EnergyInKwh;
                }
            }

            return TotalEnergyUsage;
        }*/

        public double GetTotalConsumptionInTheMoment(long deviceCategoryId)
        {
            var now = DateTime.Now;
            var totalUsage = 0.0;

            // pronalazimo sve tipove uredjaja koji pripadaju toj kategoriji
            var deviceTypeIds = _context.DeviceTypes
                .Where(dt => dt.CategoryId == deviceCategoryId)
                .Select(dt => dt.Id)
                .ToList();

            // pronalazimo sve modele uredjaja koji pripadaju tim tipovima uredjaja
            var deviceModelIds = _context.DeviceModels
                .Where(dm => deviceTypeIds.Contains(dm.DeviceTypeId))
                .Select(dm => dm.Id)
                .ToList();

            // pronalazimo sve uredjaje koji koriste te modele uredjaja
            var devices = _context.Devices
                .Where(d => deviceModelIds.Contains(d.DeviceModelId) && d.TurnOn==true)
                .Select(d => d.Id)
                .ToList();

            // pronalazimo uredjaje te kategorije u tabeli DeviceEnergyUsages i to ako trenutno rade
            var usages = _context.DeviceEnergyUsages
                .Where(u => devices.Contains(u.DeviceId) && u.StartTime <= now && (u.EndTime >= now || u.EndTime == null))
                .ToList();

            foreach (var usage in usages)
            {
                var usageStart = usage.StartTime;
                /*if (usageStart < now)
                {
                    usageStart = now;
                }*/

                var usageEnd = usage.EndTime;
                if (usageEnd == null || usageEnd > now)
                {
                    usageEnd = now;
                }

                var usageTime = (usageEnd - usageStart).TotalHours;
                var deviceEnergyUsage = _context.Devices
                    .Include(d => d.DeviceModel)
                    .Where(d => d.Id == usage.DeviceId)
                    .Select(d => d.DeviceModel.EnergyKwh)
                    .FirstOrDefault();

                totalUsage += deviceEnergyUsage * usageTime;
            }

            return Math.Round(totalUsage,2 );
        }

        public double GetTotalConsumptionInTheMomentForSettlement(long deviceCategoryId, long settlementId)
        {
            var now = DateTime.Now;
            var totalUsage = 0.0;

            // pronalazimo sve tipove uredjaja koji pripadaju toj kategoriji
            var deviceTypeIds = _context.DeviceTypes
                .Where(dt => dt.CategoryId == deviceCategoryId)
                .Select(dt => dt.Id)
                .ToList();

            // pronalazimo sve modele uredjaja koji pripadaju tim tipovima uredjaja
            var deviceModelIds = _context.DeviceModels
                .Where(dm => deviceTypeIds.Contains(dm.DeviceTypeId))
                .Select(dm => dm.Id)
                .ToList();

            // pronalazimo sve uredjaje koji koriste te modele uredjaja
            var devices = _context.Devices
                .Where(d => deviceModelIds.Contains(d.DeviceModelId))
                .Select(d => d.Id)
                .ToList();

            // pronalazimo uredjaje te kategorije u tabeli DeviceEnergyUsages i to ako trenutno rade
            var usages = _context.DeviceEnergyUsages
                .Where(u => devices.Contains(u.DeviceId) && u.StartTime <= now && (u.EndTime >= now || u.EndTime == null) && _context.Devices.Any(d => d.Id == u.DeviceId && _context.Users.Any(u => u.Id == d.UserId && u.SettlementId == settlementId)))
                .ToList();

            Console.WriteLine("******************************** COUNT: " + usages.Count);

            foreach (var usage in usages)
            {
                var usageStart = usage.StartTime;
                /*if (usageStart < now)
                {
                    usageStart = now;
                }*/

                var usageEnd = usage.EndTime;
                if (usageEnd == null || usageEnd > now)
                {
                    usageEnd = now;
                }

                var usageTime = (usageEnd - usageStart).TotalHours;
                var deviceEnergyUsage = _context.Devices
                    .Include(d => d.DeviceModel)
                    .Where(d => d.Id == usage.DeviceId)
                    .Select(d => d.DeviceModel.EnergyKwh)
                    .FirstOrDefault();

                totalUsage += deviceEnergyUsage * usageTime;
            }

            return Math.Round(totalUsage, 2);
        }

        public double GetTotalConsumptionInTheMomentForCity(long deviceCategoryId, long cityId)
        {
            var now = DateTime.Now;
            var totalUsage = 0.0;

            // pronalazimo sve tipove uredjaja koji pripadaju toj kategoriji
            var deviceTypeIds = _context.DeviceTypes
                .Where(dt => dt.CategoryId == deviceCategoryId)
                .Select(dt => dt.Id)
                .ToList();

            // pronalazimo sve modele uredjaja koji pripadaju tim tipovima uredjaja
            var deviceModelIds = _context.DeviceModels
                .Where(dm => deviceTypeIds.Contains(dm.DeviceTypeId))
                .Select(dm => dm.Id)
                .ToList();

            // pronalazimo sve uredjaje koji koriste te modele uredjaja
            var devices = _context.Devices
                .Where(d => deviceModelIds.Contains(d.DeviceModelId))
                .Select(d => d.Id)
                .ToList();

            // pronalazimo uredjaje te kategorije u tabeli DeviceEnergyUsages i to ako trenutno rade
            var usages = _context.DeviceEnergyUsages
                .Where(u => devices.Contains(u.DeviceId) && u.StartTime <= now && (u.EndTime >= now || u.EndTime == null) && _context.Devices.Any(d => d.Id == u.DeviceId && _context.Users.Any(u => u.Id == d.UserId && _context.Settlements.Any(s => s.Id == u.SettlementId && s.CityId == cityId && _context.Cities.Any(c => c.Id == s.CityId))))).ToList();

            foreach (var usage in usages)
            {
                var usageStart = usage.StartTime;

                var usageEnd = usage.EndTime;
                if (usageEnd == null || usageEnd > now)
                {
                    usageEnd = now;
                }

                var usageTime = (usageEnd - usageStart).TotalHours;
                var deviceEnergyUsage = _context.Devices
                    .Include(d => d.DeviceModel)
                    .Where(d => d.Id == usage.DeviceId)
                    .Select(d => d.DeviceModel.EnergyKwh)
                    .FirstOrDefault();

                totalUsage += deviceEnergyUsage * usageTime;
            }

            return Math.Round(totalUsage, 2);
        }

        public double GetTotalConsumptionInTheMomentForOneProsumer(long deviceCategoryId, long userId)
        {
            var now = DateTime.Now;
            var totalUsage = 0.0;

            // pronalazimo sve tipove uredjaja koji pripadaju toj kategoriji
            var deviceTypeIds = _context.DeviceTypes
                .Where(dt => dt.CategoryId == deviceCategoryId)
                .Select(dt => dt.Id)
                .ToList();

            // pronalazimo sve modele uredjaja koji pripadaju tim tipovima uredjaja
            var deviceModelIds = _context.DeviceModels
                .Where(dm => deviceTypeIds.Contains(dm.DeviceTypeId))
                .Select(dm => dm.Id)
                .ToList();

            // pronalazimo sve uredjaje koji koriste te modele uredjaja i pripadaju datom korisniku
            var devices = _context.Devices
                .Where(d => deviceModelIds.Contains(d.DeviceModelId) && d.UserId == userId && d.TurnOn == true)
                .Select(d => d.Id)
                .ToList();

            // pronalazimo uredjaje te kategorije u tabeli DeviceEnergyUsages i to ako trenutno rade
            var usages = _context.DeviceEnergyUsages
                .Where(u => devices.Contains(u.DeviceId) && u.StartTime <= now && (u.EndTime >= now || u.EndTime == null) && _context.Devices.Any(d => d.Id == u.DeviceId && d.UserId == userId)).ToList();

            foreach (var usage in usages)
            {
                var usageStart = usage.StartTime;

                var usageEnd = usage.EndTime;
                if (usageEnd == null || usageEnd > now)
                {
                    usageEnd = now;
                }

                var usageTime = (usageEnd - usageStart).TotalHours;
                var deviceEnergyUsage = _context.Devices
                    .Include(d => d.DeviceModel)
                    .Where(d => d.Id == usage.DeviceId)
                    .Select(d => d.DeviceModel.EnergyKwh)
                    .FirstOrDefault();

                totalUsage += deviceEnergyUsage * usageTime;
            }

            return Math.Round(totalUsage, 2);
        }

        public double GetNumberOfProsumersFromSettlement(long settlementId)
        {
            var totalPopulation = _context.Users
                                    .Where(u => u.SettlementId == settlementId)
                                    .Count();
             
            Console.WriteLine("++++++++++++++++Broj usera: " + totalPopulation);

            return totalPopulation;
        }

        public double GetNumberOfProsumersFromCity(long cityId)
        {
            var totalPopulation = _context.Users
                                        .Where(u => u.Settlement.City.Id == cityId)
                                        .Count();

            Console.WriteLine("++++++++++++++++Broj usera city: " + totalPopulation);

            return totalPopulation;
        }

        public double GetAverageConsumptionInTheMomentForSettlement(long settlementId, double totalEnergyUsage)
        {
            var totalPopulation = GetNumberOfProsumersFromSettlement(settlementId);
            if (totalPopulation == 0)
                return 0;
            return Math.Round(totalEnergyUsage / totalPopulation, 2);
        }

        public double GetAverageConsumptionInTheMomentForCity(long settlementId, double totalEnergyUsage)
        {
            var totalPopulation = GetNumberOfProsumersFromCity(settlementId);
            if (totalPopulation == 0)
                return 0;
            return Math.Round(totalEnergyUsage / totalPopulation, 2);
        }

        public double GetAverageConsumptionProductionInTheMomentForAllProsumers(double totalEnergyUsage)
        {
            var totalPopulation = _context.Users.Count();

            Console.WriteLine("++++++++++++++++Broj usera country: " + totalPopulation);

            if (totalPopulation == 0)
                return 0;

            return Math.Round(totalEnergyUsage / totalPopulation, 2);
        }

        public double GetTotalNumberOfDevicesInTheCity(long deviceCategoryId, long cityId)
        {
            // dodaj za grad kad se promene modeli
            double NumberOfDevices = 0.0;
            var Devices = _context.Devices.Where(d => _context.DeviceModels
                                                      .Where(dm => _context.DeviceTypes
                                                                   .Where(dt => dt.CategoryId == deviceCategoryId)
                                                                   .Select(dt => dt.Id)
                                                                   .Contains(dm.DeviceTypeId))
                                                      .Select(dm => dm.Id)
                                                      .Contains(d.DeviceModelId))
                                           .ToList();
            NumberOfDevices = Devices.Count;

            return NumberOfDevices;
        }

        public double GetTotalNumberOfDevicesInTheSettlement(long deviceCategoryId, long cityId, long settlementId)
        {
            // dodaj za naselje kad se promene modeli
            double NumberOfDevices = 0.0;
            var Devices = _context.Devices.Where(d => _context.DeviceModels
                                                      .Where(dm => _context.DeviceTypes
                                                                   .Where(dt => dt.CategoryId == deviceCategoryId)
                                                                   .Select(dt => dt.Id)
                                                                   .Contains(dm.DeviceTypeId))
                                                      .Select(dm => dm.Id)
                                                      .Contains(d.DeviceModelId))
                                           .ToList();
            NumberOfDevices = Devices.Count;

            return NumberOfDevices;
        }

        public List<EnergyToday> CalculateEnergyUsageForToday(long deviceId)
        {
            var device = _context.Devices
                .Include(d => d.DeviceModel) // da bih izvukao EnergyKwh odatle
                .FirstOrDefault(d => d.Id == deviceId);

            if (device == null)
                return null;

            var energyKwh = device.DeviceModel.EnergyKwh;
            //Console.WriteLine("+++++++++++++++++++++++++++++++++ device ID:"+device.Id);

            var Result = new List<EnergyToday>();

            // racunamo potrosnju trazenog uredjaja od 00:00h do ovog trenutka, danasnjeg dana
            var deviceEnergyUsages = _context.DeviceEnergyUsages
                .Where(usage => usage.DeviceId == deviceId && usage.StartTime.Date == DateTime.Today)
                .ToList();

            Console.WriteLine("::::::::::::::::::DateTime.Today = "+DateTime.Today);
            Console.WriteLine("::::::::::::::::::DateTime.Now = " + DateTime.Now);

            // prolazimo kroz sve sate danasnjeg dana do ovog trenutka i racunamo potrosnju za svaki sat
            for (int hour = 0; hour <= DateTime.Now.Hour; hour++)
            {
                var startTime = DateTime.Today.AddHours(hour);
                var endTime = DateTime.Today.AddHours(hour + 1);
                var energyUsageResult = 0.0;
                Console.WriteLine("++++++++++++++++++++++ startTime="+startTime+" --- endTime="+endTime);
                foreach (var usage in deviceEnergyUsages)
                {
                    Console.WriteLine("********************************** DeviceId="+usage.DeviceId+" --- StartTime="+usage.StartTime+" --- EndTime="+usage.EndTime);

                    DateTime overlapStart;
                    if (usage.StartTime < startTime)
                    {
                        overlapStart = startTime;
                    }
                    else
                    {
                        overlapStart = usage.StartTime;
                    }

                    DateTime overlapEnd;
                    if (usage.EndTime == null || usage.EndTime > endTime)
                    {
                        overlapEnd = endTime;
                    }
                    else
                    {
                        if (usage.EndTime > DateTime.Now)
                            overlapEnd = DateTime.Now;
                        else
                            overlapEnd = usage.EndTime;
                    }

                    if (overlapStart < overlapEnd)
                    {
                        var durationInHours = (overlapEnd - overlapStart).TotalHours;
                        energyUsageResult += durationInHours * energyKwh;
                    }
                }

                Result.Add(new EnergyToday
                {
                    EnergyUsageResult = Math.Round(energyUsageResult, 2),
                    Hour = startTime.Hour,
                    Day = DateTime.Now.Day,
                    Month = DateTime.Now.ToString("MMMM"),
                    Year = DateTime.Now.Year
                });
            }

            return Result;
        }

        public int GetNumberOfDevicesOfOneProsumer(long userId)
        {
            var connection = _context.Database.GetDbConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(*) FROM Devices WHERE UserId = {userId}";

            int result = Convert.ToInt32(command.ExecuteScalar()); // ne dozvoljava int jer je 64b

            connection.Close();

            return result;
        }

        public double GetUserEnergyConsumptionForToday(long userId, long deviceCategoryId)
        {
            var devicesForUser = _context.Devices
                                .Include(d => d.DeviceModel)
                                    .ThenInclude(dm => dm.DeviceType)
                                    .ThenInclude(dt => dt.DeviceCategory)
                                .Where(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == deviceCategoryId)                                
                                .ToList();

            if (devicesForUser.Count == 0)
            {
                return 0;
            }

            var deviceIds = devicesForUser.Select(d => d.Id).ToList();
            var usageList = new List<DeviceEnergyUsage>();

            var StartDate = DateTime.Today;
            var EndDate = DateTime.Now;

            usageList = _context.DeviceEnergyUsages
                        .Where(u => deviceIds.Contains(u.DeviceId) && u.StartTime >= StartDate/* && u.EndTime <= EndDate*/)
                        .ToList();

            var totalEnergyConsumption = 0.0;

            foreach (var device in devicesForUser)
            {
                var deviceUsageList = usageList.Where(u => u.Device.Id == device.Id).ToList();

                var DeviceModel = _context.DeviceModels.FirstOrDefault(dm => dm.Id == device.DeviceModelId);
                float EnergyInKwh = DeviceModel.EnergyKwh;

                foreach (var usage in deviceUsageList)
                {
                    if (usage.EndTime > EndDate)
                        usage.EndTime = EndDate;

                    totalEnergyConsumption += (usage.EndTime - usage.StartTime).TotalHours * EnergyInKwh;// device.EnergyInKwh;
                }
            }

            return Math.Round(totalEnergyConsumption, 2);
        }
    }
}
