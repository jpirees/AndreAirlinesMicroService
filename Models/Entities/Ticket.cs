using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Ticket : BaseEntity
    {
        public Passenger Passenger { get; set; }
        
        public Flight Flight { get; set; }
        
        public BasePrice BasePrice { get; set; }

        public TicketType TicketType { get; set; }

        public Decimal DiscountPercentage { get; set; }

        public Decimal TotalPrice { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
