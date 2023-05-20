using Server.Enums;

namespace Server.Filters
{
    public class ProsumerDSOFilterModel
    {
        public long? SettlmentId { get; set; }

        //Sortiranje 
        public SortCriteriaForProsumers sortCriteria { get; set; } = SortCriteriaForProsumers.Name;
        public bool SortAscending { get; set; } = true;

        //Vece od/Manje od
        public long? DeviceCategoryId { get; set; }
        public double? Value { get; set; }
        public bool? greaterThan { get; set; } = true;

        //Search za name + username + address
        public string? SearchValue { get; set; }
    }
}
