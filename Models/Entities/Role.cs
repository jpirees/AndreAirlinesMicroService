using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Models.Entities
{
    public class Role : BaseEntity
    {
        public override string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Description { get; set; }

        public List<Access> Access { get; set; }
    }
}
