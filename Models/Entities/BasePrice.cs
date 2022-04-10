using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class BasePrice : BaseEntity
    {
        public Airport AirportOrigin { get; set; }

        public Airport AirportDestination { get; set; }

        public Decimal Price { get; set; }

        public DateTime InclusionDate { get; set; }
    }
}
