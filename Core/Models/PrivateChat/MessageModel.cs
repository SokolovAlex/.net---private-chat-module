using Core.Enums.PrivateChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.PrivateChat
{
    public class MessageModel : BaseModel
    {
        public string Text { get; set; }

        public MessageStatus Status { get; set; }

        public int AuthorId { get; set; }

        public int RecipientId { get; set; }

        public bool IsMine { get; set; }

        public bool IsRead { get; set; }
    }
}
