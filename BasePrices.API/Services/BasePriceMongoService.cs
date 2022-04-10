using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Entities;
using MongoDB.Driver;
using Utils.MongoDB;

namespace BasePrices.API.Services
{
    public class BasePriceMongoService
    {
         private readonly IMongoCollection<BasePrice> _basePrices;

        public BasePriceMongoService(IMongoDatabaseSettings settings)
        {
            var basePrice = new MongoClient(settings.ConnectionString);
            var databse = basePrice.GetDatabase(settings.DatabaseName);
            _basePrices = databse.GetCollection<BasePrice>(settings.CollectionName);
        }

        public async Task<List<BasePrice>> Get() =>
           await _basePrices.Find(basePrice => true)
                           .ToListAsync();

        public async Task<BasePrice> Get(string id) =>
            await _basePrices.Find(basePrice => basePrice.Id == id)
                            .FirstOrDefaultAsync<BasePrice>();

        public async Task<List<BasePrice>> GetByAirportOrigin(string airportOriginCode) =>
            await _basePrices.Find(basePrice => basePrice.AirportOrigin.Code == airportOriginCode)
                            .ToListAsync<BasePrice>();

        public async Task<List<BasePrice>> GetByAirportDestination(string airportDestinationCode) =>
            await _basePrices.Find(basePrice => basePrice.AirportDestination.Code == airportDestinationCode)
                            .ToListAsync<BasePrice>();

        public async Task<List<BasePrice>> GetByBasePrice(string airportOriginCode, string airportDestinationCode) =>
            await _basePrices.Find(basePrice =>
                                basePrice.AirportOrigin.Code == airportOriginCode &&
                                basePrice.AirportDestination.Code == airportDestinationCode)
                          .ToListAsync<BasePrice>();

        public async Task<BasePrice> Create(BasePrice basePriceIn)
        {
            await _basePrices.InsertOneAsync(basePriceIn);
            return basePriceIn;
        }

        public async Task Update(string id, BasePrice basePriceIn) =>
            await _basePrices.ReplaceOneAsync(basePrice => basePrice.Id == id, basePriceIn);

        public async Task Remove(string id) =>
            await _basePrices.DeleteOneAsync(basePrice => basePrice.Id == id);
    }
}
