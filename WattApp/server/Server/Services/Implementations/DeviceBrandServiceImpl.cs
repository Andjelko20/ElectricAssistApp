using Server.Data;
using Server.Models.DropDowns.Devices;

namespace Server.Services.Implementations
{
    public class DeviceBrandServiceImpl : DeviceBrandService
    {
        SqliteDbContext _context;

        public DeviceBrandServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public DeviceBrand getBrandById(long brandId)
        {
            return _context.DeviceBrands.Find(brandId);
        }

        public string getBrandNameById(long brandId)
        {
            var brand = _context.DeviceBrands.FindAsync(brandId).Result;
            return brand.Name;
        }

      
    }
}
