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
            if (filter.CityId != null && filter.CityId!=-1)
                users = users.Where(src => src.Settlement.CityId == filter.CityId);
            if (filter.RoleId != null)
                users = users.Where(src => src.RoleId == filter.RoleId);

            if (filter.SortByNameAscending == true)
                users = users.OrderBy(src => src.Name);
            else
                users = users.OrderByDescending(src => src.Name);

            if(filter.SearchValue != null)
            {
                users = users.Where(src => src.Name.ToUpper().StartsWith(filter.SearchValue.ToUpper()) || src.Username.ToUpper().StartsWith(filter.SearchValue.ToUpper()) || src.Address.ToUpper().StartsWith(filter.SearchValue.ToUpper()));
               
            }


            return users;
        }
    }
}
