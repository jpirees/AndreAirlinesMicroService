using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Entities;
using MongoDB.Driver;
using Utils.MongoDB;

namespace TicketsTypes.API.Services
{
    public class TicketTypeMongoService
    {
        private readonly IMongoCollection<TicketType> _ticketsTypes;

        public TicketTypeMongoService(IMongoDatabaseSettings settings)
        {
            var ticketType = new MongoClient(settings.ConnectionString);
            var databse = ticketType.GetDatabase(settings.DatabaseName);
            _ticketsTypes = databse.GetCollection<TicketType>(settings.CollectionName);
        }

        public async Task<List<TicketType>> Get() =>
            await _ticketsTypes.Find(ticketType => true)
                            .ToListAsync();

        public async Task<TicketType> Get(string id) =>
            await _ticketsTypes.Find(ticketType => ticketType.Id == id)
                            .FirstOrDefaultAsync<TicketType>();

        public async Task<TicketType> GetByDescription(string description) =>
            await _ticketsTypes.Find(ticketType => ticketType.Description == description)
                            .FirstOrDefaultAsync<TicketType>();

        public async Task<TicketType> Create(TicketType ticketTypeIn)
        {
            await _ticketsTypes.InsertOneAsync(ticketTypeIn);
            return ticketTypeIn;
        }

        public async Task Update(string id, TicketType ticketTypeIn) =>
            await _ticketsTypes.ReplaceOneAsync(ticketType => ticketType.Id == id, ticketTypeIn);

        public async Task Remove(string id) =>
            await _ticketsTypes.DeleteOneAsync(ticketType => ticketType.Id == id);
    }
}
