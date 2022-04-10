using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Entities;
using MongoDB.Driver;
using Utils.MongoDB;

namespace Flights.API.Services
{
    public class FlightMongoService
    {
        private readonly IMongoCollection<Flight> _flights;

        public FlightMongoService(IMongoDatabaseSettings settings)
        {
            var flight = new MongoClient(settings.ConnectionString);
            var databse = flight.GetDatabase(settings.DatabaseName);
            _flights = databse.GetCollection<Flight>(settings.CollectionName);
        }

        public async Task<List<Flight>> Get() =>
           await _flights.Find(flight => true)
                           .ToListAsync();

        public async Task<Flight> Get(string id) =>
            await _flights.Find(flight => flight.Id == id)
                            .FirstOrDefaultAsync<Flight>();

        public async Task<List<Flight>> GetByAirportOrigin(string airportOriginCode) =>
            await _flights.Find(flight => flight.AirportOrigin.Code == airportOriginCode)
                            .ToListAsync<Flight>();

        public async Task<List<Flight>> GetByAirportDestination(string airportDestinationCode) =>
            await _flights.Find(flight => flight.AirportDestination.Code == airportDestinationCode)
                            .ToListAsync<Flight>();

        public async Task<Flight> GetByFlight(string registrationCode, string airportOriginCode, string airportDestinationCode) =>
            await _flights.Find(flight => 
                                flight.Airplane.RegistrationCode == registrationCode &&
                                flight.AirportOrigin.Code == airportOriginCode &&
                                flight.AirportDestination.Code == airportDestinationCode)
                          .FirstOrDefaultAsync<Flight>();

        public async Task<List<Flight>> GetByFlightFromTo(string airportOriginCode, string airportDestinationCode) =>
            await _flights.Find(flight =>
                                flight.AirportOrigin.Code == airportOriginCode &&
                                flight.AirportDestination.Code == airportDestinationCode)
                          .ToListAsync<Flight>();

        public async Task<Flight> Create(Flight flightIn)
        {
            await _flights.InsertOneAsync(flightIn);
            return flightIn;
        }

        public async Task Update(string id, Flight flightIn) =>
            await _flights.ReplaceOneAsync(flight => flight.Id == id, flightIn);

        public async Task Remove(string id) =>
            await _flights.DeleteOneAsync(flight => flight.Id == id);
    }
}
