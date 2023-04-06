using Server.Data;
using Server.Exceptions;
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
            DeviceCategory category = _context.DeviceCategories.FindAsync(categoryId).Result;
            if (category == null) throw new ItemNotFoundException("Category not found!");
            return category.Name;

        }

        public List<DeviceCategory> getAllCategories()
        {
            return _context.DeviceCategories.ToList();
        }
    }
}
