using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Models.Entities;

namespace Airports.API.Repositories
{
    public class AirportRepository : IAirportRepository
    {
        private readonly string _connection;

        public AirportRepository(string connection)
        {
            _connection = connection;
        }

        public async Task<List<Airport>> ToListAsync()
        {
            using (var db = new SqlConnection(_connection))
            {
                db.Open();
                var airports = await db.QueryAsync<Airport>(Airport.GET_ALL);
                return (List<Airport>) airports;
            }
        }

    }
}
