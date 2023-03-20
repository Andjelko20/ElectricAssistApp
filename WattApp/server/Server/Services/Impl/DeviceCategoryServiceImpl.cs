using Server.Data;

namespace Server.Services.Impl
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
            return category.Name;
        }
    }
}
