using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public abstract class Person : BaseEntity
    {

        public string Document { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public Address Address { get; set; }

        protected Person(string document, string name, DateTime birthDate, string phone, string email, Address address)
        {
            Document = document;
            Name = name;
            BirthDate = birthDate;
            Phone = phone;
            Email = email;
            Address = address;
        }
    }
}
