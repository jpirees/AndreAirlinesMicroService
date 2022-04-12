using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Entities;
using MongoDB.Driver;
using Utils.MongoDB;

namespace Users.API.Services
{
    public class UserMongoService
    {
        private readonly IMongoCollection<User> _users;

        public UserMongoService(IMongoDatabaseSettings settings)
        {
            var user = new MongoClient(settings.ConnectionString);
            var databse = user.GetDatabase(settings.DatabaseName);
            _users = databse.GetCollection<User>(settings.CollectionName);
        }

        public async Task<List<User>> Get() =>
           await _users.Find(user => true)
                           .ToListAsync();

        public async Task<User> Get(string id) =>
            await _users.Find(user => user.Id == id)
                            .FirstOrDefaultAsync<User>();

        public async Task<User> GetByDocument(string document) =>
            await _users.Find(user => user.Document == document)
                            .FirstOrDefaultAsync<User>();

        public async Task<User> GetByUsername(string username) =>
            await _users.Find(user => user.Username == username)
                            .FirstOrDefaultAsync<User>();

        public async Task<User> GetByUsernamePassword(string username, string password) =>
            await _users.Find(user => user.Username == username && user.Password == password)
                            .FirstOrDefaultAsync<User>();

        public async Task<User> Create(User userIn)
        {
            await _users.InsertOneAsync(userIn);
            return userIn;
        }

        public async Task Update(string id, User userIn) =>
            await _users.ReplaceOneAsync(user => user.Id == id, userIn);

        public async Task Remove(string id) =>
            await _users.DeleteOneAsync(user => user.Id == id);
    }
}
