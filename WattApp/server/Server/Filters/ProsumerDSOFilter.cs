using Server.DTOs;
using Server.Models;

namespace Server.Filters
{
    public class ProsumerDSOFilter
    {
        public static IQueryable<ProsumerForDSOResponseDTO> ApplyFilter(IQueryable<ProsumerForDSOResponseDTO> users, ProsumerDSOFilterModel filter)
        {
            if(filter == null)
                return users;

            if(filter.DeviceCategoryId != null)
            {
                if(filter.Value != null)
                {
                    if(filter.DeviceCategoryId == 1)
                    {
                        if(filter.greaterThan == true)
                            users = users.Where(src => src.CurrentProduction > filter.Value);
                        else
                            users = users.Where(src => src.CurrentProduction <= filter.Value);
                    }
                    else if(filter.DeviceCategoryId == 2)
                    {
                        if (filter.greaterThan == true)
                            users = users.Where(src => src.CurrentConsumption > filter.Value);
                        else
                            users = users.Where(src => src.CurrentConsumption <= filter.Value);
                    }

                }
            }
            return users;
        }


    }
}
