using Core.Enums.PrivateChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.ViewModels.PrivateChat
{
    public class MessageVm: BaseVm
    {
        public string Text { get; set; }

        public MessageStatus Status { get; set; }

        public int AuthorId { get; set; }

        public int RecipientId { get; set; }
    }
}
