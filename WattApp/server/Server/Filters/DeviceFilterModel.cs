using Microsoft.AspNetCore.Routing.Constraints;
using Server.Enums;

namespace Server.Filters
{
    public class DeviceFilterModel
    {
        public long? categoryId { get; set; }
        public long? typeId { get; set; }
        public long? brandId { get; set; }
        public bool? turnOn { get; set; }
        public bool? visibility { get; set; }
        public bool? controlability { get; set; }
        //Sortiranje
        public SortValues? sortCriteria { get; set; } = SortValues.Name;
        public bool? byAscending { get; set; } = true;
        //Veca od... manja od...
        public float? energyByKwh { get; set; } = 0;
        public bool? greaterThan { get; set; } = true;
        //Search prema imenu
        public string? searchValue { get; set; }


    }
}
