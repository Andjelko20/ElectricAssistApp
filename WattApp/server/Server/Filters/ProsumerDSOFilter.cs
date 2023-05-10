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


            return users;
        }


    }
}
