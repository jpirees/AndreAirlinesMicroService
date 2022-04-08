using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Entities;
using MongoDB.Driver;
using Utils.MongoDB;

namespace Airplanes.API.Services
{
    public class AirplaneMongoService
    {
        private readonly IMongoCollection<Airplane> _airplanes;

        public AirplaneMongoService(IMongoDatabaseSettings settings)
        {
            var airplane = new MongoClient(settings.ConnectionString);
            var databse = airplane.GetDatabase(settings.DatabaseName);
            _airplanes = databse.GetCollection<Airplane>(settings.CollectionName);
        }

        public async Task<List<Airplane>> Get() =>
            await _airplanes.Find(airplane => true)
                            .ToListAsync();

        public async Task<Airplane> Get(string id) =>
            await _airplanes.Find(airplane => airplane.Id == id)
                            .FirstOrDefaultAsync<Airplane>();
        
        public async Task<Airplane> GetByRegistrationCode(string registrationCode) =>
            await _airplanes.Find(airplane => airplane.RegistrationCode == registrationCode)
                            .FirstOrDefaultAsync<Airplane>();

        public async Task<Airplane> Create(Airplane airplaneIn)
        {
            await _airplanes.InsertOneAsync(airplaneIn);
            return airplaneIn;
        }

        public async Task Update(string id, Airplane airplaneIn) =>
            await _airplanes.ReplaceOneAsync(airplane => airplane.Id == id, airplaneIn);

        public async Task Remove(string id) =>
            await _airplanes.DeleteOneAsync(airplane => airplane.Id == id);
    }
}
