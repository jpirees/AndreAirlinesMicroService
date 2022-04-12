using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Log : BaseEntity
    {
        public User User { get; set; }

        public Object ObjectBeforeAction { get; set; }

        public Object ObjectAfterAction { get; set; }

        public string TypeAction { get; set; }

        public string CollectionNameAction { get; set; }

        public DateTime ActionDate { get; set; }

    }
}
