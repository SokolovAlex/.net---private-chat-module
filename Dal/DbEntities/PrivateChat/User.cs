using Dal.DbEntities.PrivateChat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.DbEntities
{
    public partial class User
    {
        public virtual ICollection<Message> MyMessages { get; set; }

        public virtual ICollection<Message> MessagesToMe { get; set; }
    }
}
