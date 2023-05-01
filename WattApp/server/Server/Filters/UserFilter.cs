using Microsoft.EntityFrameworkCore;
using Polly;
using Server.Data;
using Server.DTOs.Responses;
using Server.Models;

namespace Server.Filters
{
    public class UserFilter
    {
        public static IQueryable<UserModel> ApplyFilter(IQueryable<UserModel> users, UserFilterModel userFilterModel)
        {

            if (userFilterModel == null)
            {
                return users;
            }

            if (userFilterModel.Blocked != null)
            {
                users = users.Where(src => src.Blocked == userFilterModel.Blocked);
            }
            if(userFilterModel.RoleId != null)
            {
                users = users.Where(src => src.RoleId == userFilterModel.RoleId);
            }
            if(userFilterModel.CityId != null)
            {
                //MOZE POTENCIJALNO BITI PROBLEM !!!!!!!!!
                users = users.Where(src => src.Settlement.CityId == userFilterModel.CityId);
            }
            if(userFilterModel.SettlementId != null)
            {
                users = users.Where(src => src.SettlementId == userFilterModel.SettlementId);
            }

            //Sortiranje
            if(userFilterModel.SortByNameAscending != null)
            {
                if(userFilterModel.SortByNameAscending == true)
                {
                    users = users.OrderBy(src => src.Name);
                }
                else if(userFilterModel.SortByNameAscending == true)
                {
                    users = users.OrderByDescending(src => src.Name);
                }
            }

            //Vrati korisnike provucene kroz filtere
            return users;
        }
    }
}
