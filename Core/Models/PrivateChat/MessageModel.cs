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

        public DateTime? ReadDate { get; set; }

        public string DisplayCreateDate { get {
                return String.Format("{0} {1}", CreateDate.ToShortDateString(), CreateDate.ToShortTimeString());
            }
        }

        public string DisplayReadDate
        {
            get
            {
                if (!ReadDate.HasValue) {
                    return "Not read yet.";
                }
                return String.Format("{0} {1}", ReadDate.Value.ToShortDateString(), ReadDate.Value.ToShortTimeString());
            }
        }
    }
}
