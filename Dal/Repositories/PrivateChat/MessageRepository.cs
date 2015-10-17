using AutoMapper;
using Core.Enums.PrivateChat;
using Core.Models;
using Core.Models.PrivateChat;
using Dal.DbEntities.PrivateChat;
using Dal.Repositories.IRepositories;
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
        public MessageRepository() {
            Mapper.CreateMap<Message, MessageModel>();
            Mapper.CreateMap<MessageModel, Message>();
        }

        protected override DbSet<Message> GetTable()
        {
            return context.Messages;
        }

        private IQueryable<Message> GetMyMessagesToHim(int myId, int hisId)
        {
            return context.Messages.Where(x => x.AuthorId == myId && x.RecipientId == hisId);
        }

        private IQueryable<Message> GetHisMessagesToMe(int myId, int hisId)
        {
            return context.Messages.Where(x => x.AuthorId == hisId && x.RecipientId == myId);
        }

        public IEnumerable<MessageModel> GetMessagesBetweenUsers(int myId, int hisId, PagingInfo pagingInfo)
        {
            return GetMyMessagesToHim(myId, hisId)
                .Union(GetHisMessagesToMe(myId, hisId))
                .Select(x => new MessageModel
                {
                    Id = x.Id,
                    Text = x.Text,
                    IsMine = x.AuthorId == myId,
                    IsRead = x.StatusId == (int)MessageStatus.Read,
                    CreateDate = x.CreateDate
                })
                .OrderByDescending(x => x.CreateDate)
                .Skip(pagingInfo.CurrentPage * pagingInfo.ItemsPerPage)
                .Take(pagingInfo.ItemsPerPage)
                .OrderBy(x => x.CreateDate);
        }

        public MessageModel CreateMessage(int authorId, int recipientId, string text)
        {
            var message = new Message
            {
                AuthorId = authorId,
                CreateDate = DateTime.Now,
                RecipientId = recipientId,
                Text = text,
                StatusId = (int)MessageStatus.Sent
            };

            context.Messages.Add(message);

            context.SaveChanges();

            return Mapper.Map<Message, MessageModel>(message);
        }

        public int GetCount(int myId, int hisId)
        {
            return GetMyMessagesToHim(myId, hisId)
                .Union(GetHisMessagesToMe(myId, hisId)).Count();
        }

        public IEnumerable<MessageModel> MarkAsRead(int readerId, int opponentId)
        {
            var messagesToMe = GetHisMessagesToMe(readerId, opponentId).Where(x => x.StatusId == (int)MessageStatus.Sent);

            foreach (var mes in messagesToMe)
            {
                mes.StatusId = (int)MessageStatus.Read;
            }

            context.SaveChanges();

            return messagesToMe.Select(x => new MessageModel
            {
                CreateDate = x.CreateDate,
                IsRead = true,
                IsMine = false,
                Text = x.Text,
                Id = x.Id
            });
        }
    }
}
