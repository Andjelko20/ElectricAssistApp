using Server.Data;
using Server.DTOs;
using Server.Exceptions;
using Server.Models.DropDowns.Devices;

namespace Server.Services.Implementations
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
            DeviceType type = _context.DeviceTypes.FindAsync(typeId).Result;
            if (type == null) throw new ItemNotFoundException("Brand not found!");
            return type.Name;

        }
    }
}
