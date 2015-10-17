using Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.PrivateChat
{
    public class PrivateChatRoom
    {
        public UserModel Author { get; set; }

        public UserModel Recipient { get; set; }

        public int MessagesAmount { get; set; }

        public IEnumerable<MessageModel> Messages { get; set; }

        public string MessagesJson {
            get {
                return MessagesToJsonObject();
            }
        }

        public string MessagesToJsonObject() {
            return Newtonsoft.Json.JsonConvert.SerializeObject(Messages.Select(x => new
            {
                text = x.Text,
                date = x.DisplayCreateDate,
                isMine = x.IsMine,
                isREad = x.IsRead
            }));
        }
    }
}
