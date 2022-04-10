using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Entities;
using MongoDB.Driver;
using Utils.MongoDB;

namespace Tickets.API.Services
{
    public class TicketMongoService
    {
        private readonly IMongoCollection<Ticket> _tickets;

        public TicketMongoService(IMongoDatabaseSettings settings)
        {
            var ticket = new MongoClient(settings.ConnectionString);
            var databse = ticket.GetDatabase(settings.DatabaseName);
            _tickets = databse.GetCollection<Ticket>(settings.CollectionName);
        }

        public async Task<List<Ticket>> Get() =>
           await _tickets.Find(ticket => true)
                           .ToListAsync();

        public async Task<Ticket> Get(string id) =>
            await _tickets.Find(ticket => ticket.Id == id)
                            .FirstOrDefaultAsync<Ticket>();

        public async Task<List<Ticket>> GetByFlight(string flightId) =>
           await _tickets.Find(ticket => ticket.Flight.Id == flightId)
                         .ToListAsync<Ticket>();

        public async Task<Ticket> GetByPassengerOnFlight(string passaportNumber, string flightId) =>
            await _tickets.Find(ticket => ticket.Passenger.PassaportNumber == passaportNumber &&
                                          ticket.Flight.Id == flightId)
                          .FirstOrDefaultAsync<Ticket>();

        public async Task<Ticket> Create(Ticket ticketIn)
        {
            await _tickets.InsertOneAsync(ticketIn);
            return ticketIn;
        }

        public async Task Update(string id, Ticket ticketIn) =>
            await _tickets.ReplaceOneAsync(ticket => ticket.Id == id, ticketIn);

        public async Task Remove(string id) =>
            await _tickets.DeleteOneAsync(ticket => ticket.Id == id);
    }
}
