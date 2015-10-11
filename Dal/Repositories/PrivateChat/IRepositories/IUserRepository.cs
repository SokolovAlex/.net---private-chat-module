using Core.Models;
using Core.Models.PrivateChat;
using Core.Models.User;
using Dal.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repositories.IRepositories
{
    public partial interface IUserRepository: IBaseRepository<User>
    {
        IEnumerable<UserModel> GetAllRecipientsFor(int id);

        PrivateChatRoom GetChatUsersByHashes(Guid senderHash, Guid recipientHash);
    }
}
