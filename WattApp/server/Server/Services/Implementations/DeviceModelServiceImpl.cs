using Server.Data;

namespace Server.Services.Implementations
{
    public class DeviceModelServiceImpl : DeviceModelService
    {
        SqliteDbContext _context;

        public DeviceModelServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public string getModelNameById(long modelId)
        {
            var model = _context.DeviceModels.FindAsync(modelId).Result;
            return model.Mark;
        }
    }
}
