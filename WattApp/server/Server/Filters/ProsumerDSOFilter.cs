using Server.DTOs;
using Server.Enums;
using Server.Models;

namespace Server.Filters
{
    public class ProsumerDSOFilter
    {
        public static IQueryable<ProsumerForDSOFilterDTO> ApplyFilter(IQueryable<ProsumerForDSOFilterDTO> users, ProsumerDSOFilterModel filter)
        {
            if(filter == null)
                return users;

            if (filter.SettlmentId != null)
                users = users.Where(src => src.userModel.SettlementId == filter.SettlmentId);

            if(filter.sortCriteria != null)
            {
                switch (filter.sortCriteria)
                {
                    case SortCriteriaForProsumers.Name:
                        if (filter.SortAscending == true)
                            users = users.OrderBy(src => src.userModel.Name);
                        else
                            users = users.OrderByDescending(src => src.userModel.Name);
                        break;
                    case SortCriteriaForProsumers.Consumption:
                        if (filter.SortAscending == true)
                            users = users.OrderBy(src => src.CurrentConsumption);
                        else
                            users = users.OrderByDescending(src => src.CurrentConsumption);
                        break;
                    case SortCriteriaForProsumers.Production:
                        if (filter.SortAscending == true)
                            users = users.OrderBy(src => src.CurrentProduction);
                        else
                            users = users.OrderByDescending(src => src.CurrentProduction);
                        break;
                }
            }

            if (filter.SearchValue != null)
            {
                users = users.Where(src => src.userModel.Name.ToUpper().StartsWith(filter.SearchValue.ToUpper()) || src.userModel.Username.ToUpper().StartsWith(filter.SearchValue.ToUpper()) || src.userModel.Address.ToUpper().StartsWith(filter.SearchValue.ToUpper()));

            }

            if (filter.DeviceCategoryId != null)
            {
                if(filter.Value != null)
                {
                    if(filter.DeviceCategoryId == 1)
                    {
                        if(filter.greaterThan == true)
                            users = users.Where(src => src.CurrentProduction >= filter.Value);
                        else
                            users = users.Where(src => src.CurrentProduction <= filter.Value);
                    }
                    else if(filter.DeviceCategoryId == 2)
                    {
                        if (filter.greaterThan == true)
                            users = users.Where(src => src.CurrentConsumption >= filter.Value);
                        else
                            users = users.Where(src => src.CurrentConsumption <= filter.Value);
                    }

                }
            }

            return users;
        }


    }
}
