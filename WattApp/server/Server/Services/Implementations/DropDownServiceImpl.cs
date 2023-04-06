using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;

namespace Server.Services.Implementations
{

    public class DropDownServiceImpl : DropDownService
    {
        private readonly SqliteDbContext _db;
        private readonly DeviceBrandService _deviceBrandService;
        private readonly DeviceModelService _deviceModelService;
        public DropDownServiceImpl(SqliteDbContext db, DeviceBrandService deviceBrandService, DeviceModelService deviceModelService)
        {
            _db = db;
            _deviceBrandService = deviceBrandService;
            _deviceModelService = deviceModelService;
        }

        public List<Settlement> getSettlements(long cityId)
        {
            return _db.Settlements.Where(t => t.CityId == cityId).ToList();
        }

        public List<City> getCities(long countryId)
        {
            return _db.Cities.Where(c => c.CountryId == countryId).ToList();
        }

        public List<Country> getCountries()
        {
            return _db.Countries.ToList();
        }

        public List<DeviceBrand> getDeviceBrand(long deviceTypeId)
        {
            /*
            //List<TypeBrand> brands = _db.TypeBrands.Where(x => x.TypeId == deviceTypeId).ToList();
            List<DeviceBrand> devices = new List<DeviceBrand>();
            //foreach(TypeBrand brand in brands)
            //{
            //    DeviceBrand deviceBrand = _deviceBrandService.getBrandById(brand.BrandId);
            //    devices.Add(deviceBrand);
            //}
            return devices;
            */
            List<DeviceBrand> devices=new List<DeviceBrand>();
            string connectionString = _db.Database.GetConnectionString();
            SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            SqliteCommand command = new SqliteCommand(@"SELECT Id,Name
            FROM DeviceBrands br
            WHERE EXISTS(SELECT * FROM DeviceModels m WHERE m.DeviceBrandId=br.Id AND m.DeviceTypeId=@typeId )",connection);
            command.Parameters.AddWithValue("@typeId",deviceTypeId);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                devices.Add(new DeviceBrand {
                    Id = long.Parse(reader["Id"].ToString()),
                    Name = reader["Name"].ToString()
                });
            }
            connection.Close();
            return devices;
        }

        public List<DeviceCategory> getDeviceCategories()
        {
            return _db.DeviceCategories.ToList();
        }

        public List<DeviceModel> getDeviceModel(long deviceTypeId, long deviceBrandId)
        {
            //List<TypeBrandModel> models = _db.TypeBrandModels.Where(x => x.TypeId == deviceTypeId && x.BrandId == deviceBrandId).ToList();
            List<DeviceModel> deviceModels = //new List<DeviceModel>();
            _db.DeviceModels.FromSqlRaw(@"SELECT * FROM DeviceModels
            WHERE DeviceTypeId="+deviceTypeId+" AND DeviceBrandId="+deviceBrandId).ToList();
            //foreach(TypeBrandModel model in models)
            //{
            //    DeviceModel deviceModel = _deviceModelService.getDeviceModel(model.ModelId);
            //    deviceModels.Add(deviceModel);
            //}
            return deviceModels;
        }

        public List<DeviceType> getDeviceTypes(long deviceCategoryId)
        {
            return _db.DeviceTypes.Where(x => x.CategoryId == deviceCategoryId).ToList();
        }

        
    }
}
