using Server.Models;

namespace Server.Filters
{
    public class UserFilter
    {
        public static IQueryable<UserModel> applyFilters(IQueryable<UserModel> users, UserFilterModel filter)
        {
            if (filter == null)
                return users;
            if (filter.Blocked != null)
                users = users.Where(src => src.Blocked == filter.Blocked);
            if (filter.SettlmentId != null)
                users = users.Where(src => src.SettlementId == filter.SettlmentId);
            if (filter.CityId != null)
                users = users.Where(src => src.Settlement.CityId == filter.CityId);
            if (filter.RoleId != null)
                users = users.Where(src => src.RoleId == filter.RoleId);


            return users;
        }
    }
}
