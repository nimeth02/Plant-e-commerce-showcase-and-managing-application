using crud_application.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class CarService
{
    private readonly IMongoCollection<CarModel> _cars;

    public CarService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        var client = new MongoClient("mongodb://localhost:27017/");
        var database = client.GetDatabase("CarRent");
        _cars = database.GetCollection<CarModel>("CarRentCollection");
    }
   
    public async Task<List<CarModel>> GetCars()
    {
        return await _cars.Find(_ => true).ToListAsync();
    }

    public async Task<CarModel> GetCar(string id)
    {
        return await _cars.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateCar(CarModel car)
    {
        await _cars.InsertOneAsync(car);
    }

    public async Task UpdateCar(string id, CarModel car)
    {
        await _cars.ReplaceOneAsync(p => p.Id == id, car);
    }

    public async Task DeleteCar(string id)
    {
        await _cars.DeleteOneAsync(p => p.Id == id);
    }
}
