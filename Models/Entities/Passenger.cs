using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Passenger : Person
    {
        public string PassaportNumber { get; set; }

        public Passenger(string document, string name, DateTime birthDate, string phone, string email, Address address, string passaportNumber) : base(document, name, birthDate, phone, email, address)
        {
            PassaportNumber = passaportNumber;
        }
    }
}
