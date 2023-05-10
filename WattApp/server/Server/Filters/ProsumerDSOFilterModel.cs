namespace Server.Filters
{
    public class ProsumerDSOFilterModel
    {
        public bool? Blocked { get; set; }
        public long? RoleId { get; set; }
        public long? SettlmentId { get; set; }
        //Sortiranje 
        public bool SortByNameAscending { get; set; } = true;

        //Search za name + username + address
        public string? SearchValue { get; set; }

        //Vece od/Manje od
        public long? DeviceCategoryId { get; set; }
        public double? Value { get; set; }
        public bool? greaterThan { get; set; } = true;
    }
}
