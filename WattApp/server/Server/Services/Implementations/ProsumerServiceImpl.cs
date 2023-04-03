using Server.Data;
using Server.Models;

namespace Server.Services.Implementations
{
    public class ProsumerServiceImpl : IProsumerService
    {
        private readonly SqliteDbContext _context;
        public ProsumerServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        /*public double GetTotalConsumptionInTheMoment()
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
    }
}
