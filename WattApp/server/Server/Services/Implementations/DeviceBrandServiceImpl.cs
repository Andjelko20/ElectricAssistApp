using Microsoft.AspNetCore.Server.IIS.Core;
using Server.Data;
using Server.Exceptions;
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

        public string getBrandNameById(long brandId)
        {
            DeviceBrand brand = _context.DeviceBrands.FindAsync(brandId).Result;
            if (brand == null) throw new ItemNotFoundException("Brand not found!");
            return brand.Name;
        }

      
    }
}
