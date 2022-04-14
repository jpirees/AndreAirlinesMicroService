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

        public string ObjectBeforeAction { get; set; }

        public string ObjectAfterAction { get; set; }

        public string TypeAction { get; set; }

        public string CollectionNameAction { get; set; }

        public DateTime ActionDate { get; set; }

        public Log(User user, string objectBeforeAction, string objectAfterAction, string typeAction, string collectionNameAction)
        {
            User = user;
            ObjectBeforeAction = objectBeforeAction;
            ObjectAfterAction = objectAfterAction;
            TypeAction = typeAction;
            CollectionNameAction = collectionNameAction;
            ActionDate = DateTime.Now;
        }
    }
}
