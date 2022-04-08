using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class User : Person
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Department { get; set; }

        public Role Role { get; set; }

        public User(string document, string name, DateTime birthDate, string phone, string email, Address address, string username, string password, string department, Role role) : base(document, name, birthDate, phone, email, address)
        {
            Username = username;
            Password = password;
            Department = department;
            Role = role;
        }

    }
}
