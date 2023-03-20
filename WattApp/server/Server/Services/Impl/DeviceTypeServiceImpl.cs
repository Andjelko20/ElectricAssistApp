using Server.Data;
using Server.DTOs;

namespace Server.Services.Impl
{
    public class DeviceTypeServiceImpl : DeviceTypeService
    {
        SqliteDbContext _context;

        public DeviceTypeServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public string getTypeNameById(long typeId)
        {
            var type = _context.DeviceTypes.FindAsync(typeId).Result;
            return type.Name;
        }
    }
}
