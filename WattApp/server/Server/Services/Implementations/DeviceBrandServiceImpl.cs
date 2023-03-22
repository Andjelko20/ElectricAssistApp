using Server.Data;

namespace Server.Services.Impl
{
    public class DeviceBrandServiceImpl : DeviceBrandService
    {
        SqliteDbContext _context;

        public DeviceBrandServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public string getBrandNameById(long brandId)
        {
            var brand = _context.DeviceBrands.FindAsync(brandId).Result;
            return brand.Name;
        }
    }
}
