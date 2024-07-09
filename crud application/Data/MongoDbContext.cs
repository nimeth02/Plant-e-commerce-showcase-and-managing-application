using crud_application.configuration;
using crud_application.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace crud_application.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        private readonly ILogger<MongoDbContext> _logger;
        public MongoDbContext(IOptions<MongoDbConfiguration> settings, ILogger<MongoDbContext> logger)
        {
            _logger = logger;
            _logger.LogWarning("in creating car.");
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<CarModel> CarModels => _database.GetCollection<CarModel>("CarRentCollection");
    }
}
