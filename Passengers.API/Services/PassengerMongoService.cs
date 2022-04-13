using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Entities;
using MongoDB.Driver;
using Utils.MongoDB;

namespace Passengers.API.Services
{
    public class PassengerMongoService
    {
        private readonly IMongoCollection<Passenger> _passengers;

        public PassengerMongoService(IMongoDatabaseSettings settings)
        {
            var passenger = new MongoClient(settings.ConnectionString);
            var database = passenger.GetDatabase(settings.DatabaseName);
            _passengers = database.GetCollection<Passenger>(settings.CollectionName);
        }

        public async Task<List<Passenger>> Get() =>
           await _passengers.Find(passenger => true)
                           .ToListAsync();

        public async Task<Passenger> Get(string id) =>
            await _passengers.Find(passenger => passenger.Id == id)
                            .FirstOrDefaultAsync<Passenger>();

        public async Task<Passenger> GetByDocument(string document) =>
            await _passengers.Find(passenger => passenger.Document == document)
                            .FirstOrDefaultAsync<Passenger>();

        public async Task<Passenger> GetByPassaportNumber(string passaportNumber) =>
            await _passengers.Find(passenger => passenger.PassaportNumber == passaportNumber)
                            .FirstOrDefaultAsync<Passenger>();

        public async Task<Passenger> Create(Passenger passengerIn)
        {
            await _passengers.InsertOneAsync(passengerIn);
            return passengerIn;
        }

        public async Task Update(string id, Passenger passengerIn) =>
            await _passengers.ReplaceOneAsync(passenger => passenger.Id == id, passengerIn);

        public async Task Remove(string id) =>
            await _passengers.DeleteOneAsync(passenger => passenger.Id == id);
    }
}
