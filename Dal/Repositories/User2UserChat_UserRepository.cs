using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.BLL.Common.Models.User;
using App.BLL.Common.Models.User2UserChat;
using App.DAL.DataBaseModel;
using EmitMapper;

namespace App.DAL.Concrete.Repos
{
    public partial class UserRepository
    {
        public IEnumerable<UserModel> GetAllRecipientsFor(int id)
        {
            var dbUsers = _context.USER.Where(x => x.ID != id).ToList();
            return dbUsers.Select(x => ObjectMapperManager.DefaultInstance.GetMapper<USER, UserModel>().Map(x));
        }

        public PrivateChatUsers GetChatUsersByHashes(string senderHash, string recipientHash)
        {
            var users = _context.USER.Where(x => x.HashID == senderHash || x.HashID == recipientHash).ToList();
            var sender = users.FirstOrDefault(x => x.HashID == senderHash);
            var recipient = users.FirstOrDefault(x => x.HashID == recipientHash);

            var result = new PrivateChatUsers();

            if (sender == null || recipient == null)
            {
                return result;
            }

            result.Sender = ObjectMapperManager.DefaultInstance.GetMapper<USER, UserModel>().Map(sender);
            result.Recipient = ObjectMapperManager.DefaultInstance.GetMapper<USER, UserModel>().Map(recipient);

            return result;
        }
    }
}
