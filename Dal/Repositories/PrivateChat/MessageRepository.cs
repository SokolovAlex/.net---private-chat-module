using Dal.DbEntities.PrivateChat;
using Dal.Repositories.IRepositories;
using Ssibir.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repositories.PrivatChat
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        protected override DbSet<Message> GetTable()
        {
            return context.Messages;
        }
    }
}
