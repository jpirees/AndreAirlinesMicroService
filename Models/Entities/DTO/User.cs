using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities.DTO
{
    public class UserRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserResponseDTO : BaseEntity
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Department { get; set; }
        public Role Role { get; set; }

        public UserResponseDTO(string name, string username, string department, Role role)
        {
            Name = name;
            Username = username;
            Department = department;
            Role = role;
        }
    }


}
