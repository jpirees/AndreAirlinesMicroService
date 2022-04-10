using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Models.Entities
{
    public class TicketType : BaseEntity
    {
        public string Description { get; set; }

        public Decimal Price { get; set; }
    }
}
