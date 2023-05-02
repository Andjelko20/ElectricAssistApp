namespace Server.Filters
{
    public class UserFilterModel
    {
        //Kriterijumi za filtriranje
        public bool? Blocked { get; set; }
        public long? RoleId { get; set; }
        public long? SettlmentId { get; set; }   
        public long? CityId { get; set; }

        //Sortiranje 
        public bool SortByNameAscending { get; set; } = true;


    }
}
