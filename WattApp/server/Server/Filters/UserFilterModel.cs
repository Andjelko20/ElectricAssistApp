namespace Server.Filters
{
    public class UserFilterModel
    {
        //Filteri
        public long RoleId { get; set; }
        public bool Blocked { get; set; }
        public long CityId { get; set; }
        public long SettlementId { get; set; }

        //Sortiranje
        public bool SortByNameAscending { get; set; } = true; //od A-Z
        

    }
}
