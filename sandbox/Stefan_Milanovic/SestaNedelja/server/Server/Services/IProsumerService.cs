namespace Server.Services
{
    public interface IProsumerService
    {
        public double GetTotalConsumptionInTheMoment();
        public double GetTotalNumberOfDevicesInTheCity(long deviceCategoryId, long countryId);
    }
}
