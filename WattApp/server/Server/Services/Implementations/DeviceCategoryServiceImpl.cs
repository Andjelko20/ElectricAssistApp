using Server.Data;
using Server.Models.DropDowns.Devices;

namespace Server.Services.Implementations
{
    public class DeviceCategoryServiceImpl : DeviceCategoryService
    {
        SqliteDbContext _context;

        public DeviceCategoryServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }


        public string getCategoryNameById(long categoryId)
        {
            var category = _context.DeviceCategories.FindAsync(categoryId).Result;
            return category == null ? null : category.Name;
        }

        public List<DeviceCategory> getAllCategories()
        {
            return _context.DeviceCategories.ToList();
        }
    }
}
