namespace Server.DTOs
{
    public class ForByHourPaginationWithNumberOfRows
    {
        public int NumberOfRows { get; set; }
        public int NumberOfPages { get; set; }
        public List<EnergyToday> ByHourPaginationList {get; set;}
    }
}
