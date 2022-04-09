using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Entities;

namespace Airports.API.Repositories
{
    public interface IAirportRepository
    {
        public Task<List<Airport>> ToListAsync();
    }
}
