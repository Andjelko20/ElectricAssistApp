using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models.DropDowns.Location;
using System.Collections.Generic;

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

        public List<SettlementDTO> GetSettlements(long cityId)
        {
            var settlements = _context.Settlements.Where(s => s.CityId == cityId).Select(s => new SettlementDTO { Id=s.Id, Name=s.Name }).ToList();
            return settlements;
        }
    }
}
