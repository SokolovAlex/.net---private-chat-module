using AutoMapper;
using Core.Models.PrivateChat;
using Core.Models.User;
using Dal.DbEntities;
using Dal.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repositories.Repositories
{
    public partial class UserRepository : BaseRepository<User>, IUserRepository
    {
        public IEnumerable<UserModel> GetAllRecipientsFor(int id)
        {
            var dbUsers = GetTable().Where(x => x.Id != id).ToList();
            return Mapper.Map<IEnumerable<UserModel>>(dbUsers);
        }

        public PrivateChatRoom GetChatUsersByHashes(Guid senderHash, Guid recipientHash)
        {
            var users = GetTable().Where(x => x.Hash == senderHash || x.Hash == recipientHash).ToList();
            var sender = users.FirstOrDefault(x => x.Hash == senderHash);
            var recipient = users.FirstOrDefault(x => x.Hash == recipientHash);

            var result = new PrivateChatRoom();

            if (sender == null || recipient == null)
            {
                return result;
            }

            result.Author = Mapper.Map<UserModel>(sender);
            result.Recipient = Mapper.Map<UserModel>(recipient);

            return result;
        }
    }
}
