using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Flight : BaseEntity
    {
        public Airplane Airplane { get; set; }

        public Airport AirportOrigin { get; set; }

        public Airport AirportDestination { get; set; }

        public DateTime BoardingTime { get; set; }

        public DateTime LandingTime { get; set; }
    }
}
