using Server.DTOs;

namespace Server.Services
{
    public interface IDSOService
    {
        public long GetCityId(string cityName);
        public List<SettlementDTO> GetSettlements(long cityId);
    }
}
