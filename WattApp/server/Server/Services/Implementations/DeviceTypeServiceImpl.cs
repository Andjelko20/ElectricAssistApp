using Server.Data;
using Server.DTOs;
using Server.Models.DropDowns.Devices;

namespace Server.Services.Impl
{
    public class DeviceTypeServiceImpl : DeviceTypeService
    {
        SqliteDbContext _context;

        public DeviceTypeServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public List<DeviceType> GetDeviceTypesByCategory(long categoryId)
        {
            return _context.DeviceTypes.Where(x => x.CategoryId == categoryId).ToList();
        }

        public string getTypeNameById(long typeId)
        {
            var type = _context.DeviceTypes.FindAsync(typeId).Result;
            return type.Name;
        }
    }
}
