using Core.Models;
using Core.Models.PrivateChat;
using Dal.DbEntities.PrivateChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repositories.IRepositories
{
    public interface IMessageRepository : IBaseRepository<Message>
    {

        IEnumerable<MessageModel> GetMessagesBetweenUsers(int myId, int hisId, PagingInfo pagingInfo);

        MessageModel CreateMessage(int authorId, int recipientId, string text);

        int GetCount(int myId, int hisId);

        IEnumerable<MessageModel> MarkAsRead(int readerId, int opponentId);
    }
}
