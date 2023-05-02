namespace Server.Filters
{
    public class UserFilterModel
    {
        public bool? Blocked { get; set; }
        public long? RoleId { get; set; }
        public long? SettlmentId { get; set; }   
        public long? CityId { get; set; }
    }
}
