using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Services.Implementations
{
    public class DSOServiceImpl : IDSOService
    {
        private readonly SqliteDbContext _context;
        public DSOServiceImpl(SqliteDbContext context)
        {
            _context = context;
        }

        public long GetCityId(string cityName)
        {
            var city = _context.Cities.FirstOrDefault(c => EF.Functions.Like(c.Name, cityName));

            if (city == null) 
                return -1;
            return city.Id;
        }
    }
}
