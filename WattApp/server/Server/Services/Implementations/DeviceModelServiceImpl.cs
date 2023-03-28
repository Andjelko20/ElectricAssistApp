using Server.Data;
using Server.Exceptions;
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
            DeviceModel model = _context.DeviceModels.FindAsync(modelId).Result;
            if (model == null) throw new ItemNotFoundException("Model not found!");
            return model.Mark;

        }
    }
}
