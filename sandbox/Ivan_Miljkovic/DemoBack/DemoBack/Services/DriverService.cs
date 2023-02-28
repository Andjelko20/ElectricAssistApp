using DemoBack.Database;
using DemoBack.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DemoBack.Services
{
    public class DriverService
    {
        private readonly IMongoCollection<Driver> _driverCollection;

        public DriverService(IOptions<DatabaseConfig> databaseConfig)
        {
            var mongoClient = new MongoClient(databaseConfig.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(databaseConfig.Value.DatabaseName);
            _driverCollection = mongoDb.GetCollection<Driver>(databaseConfig.Value.CollectionName);
        }
        public async Task<List<Driver>> GetAsync() => await _driverCollection.Find(_ => true).ToListAsync();
        public async Task<Driver> GetAsync(string id) => await _driverCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task CreateAsync(Driver driver) => await _driverCollection.InsertOneAsync(driver);

        public async Task UpdateAsync(string id, Driver driver) => await _driverCollection.ReplaceOneAsync(x => x.Id == id, driver);

        public async Task RemoveAsync(string id) => await _driverCollection.DeleteOneAsync(x => x.Id == id);

    }
}
