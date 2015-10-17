using Core.Enums.PrivateChat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.DbEntities.PrivateChat
{
    public class Message: BaseEntity
    {
        public string Text { get; set; }

        [NotMapped]
        public MessageStatus Status { get; set; }
       
        public int StatusId {
            get {
                return (int)Status;
            }
            set {
                Status = (MessageStatus)value; 
            }
        }

        public int AuthorId { get; set; }

        public virtual User Author { get; set; }

        public int RecipientId { get; set; }

        public DateTime? ReadDate { get; set; }

        public virtual User Recipient { get; set; }
    }
}
