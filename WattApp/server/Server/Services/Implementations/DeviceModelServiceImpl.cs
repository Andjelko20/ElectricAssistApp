using Server.Data;
using Server.Models.DropDowns.Devices;

namespace Server.Services.Implementations
{
    public class DeviceModelServiceImpl : DeviceModelService
    {
        SqliteDbContext _context;

        public DeviceModelServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public DeviceModel getDeviceModel(long modelId)
        {
            return _context.DeviceModels.Find(modelId);
        }

        public string getModelNameById(long modelId)
        {
            var model = _context.DeviceModels.FindAsync(modelId).Result;
            return model.Mark;
        }
    }
}
