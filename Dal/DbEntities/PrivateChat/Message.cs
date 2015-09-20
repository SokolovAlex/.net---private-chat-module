using Core.Enums.PrivateChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.DbEntities.PrivateChat
{
    public class Message: BaseEntity
    {
        public string Text { get; set; }

        public MessageStatus Status { get; set; }

        public int AuthorId { get; set; }

        public virtual User Author { get; set; }

        public int RecipientId { get; set; }

        public virtual User Recipient { get; set; }
    }
}
