namespace Server.DTOs
{
    public class ForByDayPaginationWithNumberOfRowsAndPages
    {
        public int NumberOfRows { get; set; }
        public int NumberOfPages { get; set; }
        public List<DailyEnergyConsumptionPastMonth> Data { get; set; }
    }
}
