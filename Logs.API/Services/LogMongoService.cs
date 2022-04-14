using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Entities;
using MongoDB.Driver;
using Utils.MongoDB;

namespace Logs.API.Services
{
    public class LogMongoService
    {
        private readonly IMongoCollection<Log> _logs;

        public LogMongoService(IMongoDatabaseSettings settings)
        {
            var log = new MongoClient(settings.ConnectionString);
            var database = log.GetDatabase(settings.DatabaseName);
            _logs = database.GetCollection<Log>(settings.CollectionName);
        }

        public async Task<List<Log>> Get() =>
            await _logs.Find(log => true)
                            .ToListAsync();

        public async Task<Log> Get(string id) =>
            await _logs.Find(log => log.Id == id)
                            .FirstOrDefaultAsync<Log>();

        public async Task<Log> Create(Log logIn)
        {
            await _logs.InsertOneAsync(logIn);
            return logIn;
        }

        public async Task Update(string id, Log logIn) =>
            await _logs.ReplaceOneAsync(log => log.Id == id, logIn);

        public async Task Remove(string id) =>
            await _logs.DeleteOneAsync(log => log.Id == id);
    }

}
