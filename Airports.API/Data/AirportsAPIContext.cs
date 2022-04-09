using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Airports.API.Data
{
    public class AirportsAPIContext : DbContext
    {
        public AirportsAPIContext (DbContextOptions<AirportsAPIContext> options)
            : base(options)
        {
        }




        public DbSet<Models.Entities.Airport> Airport { get; set; }
    }
}
