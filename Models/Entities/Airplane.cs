using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Airplane : BaseEntity
    {
        public string RegistrationCode { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }
    }
}
