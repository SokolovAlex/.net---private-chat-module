using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.BLL.Common.Abstract.Repos;
using App.BLL.Common.Enums;
using App.BLL.Common.Models.Helper;
using App.BLL.Common.Models.User;
using App.BLL.Common.Models.User2UserChat;
using App.DAL.DataBaseModel;
using EmitMapper;

namespace App.DAL.Concrete.Repos
{
    public class MessageRepository: BaseRepository, IMessageRepository
    {
        private IQueryable<ChatMessage> GetMyMessagesToHim(int myId, int hisId)
        {
            return _context.ChatMessage.Where(x => x.AuthorId == myId && x.RecipientId == hisId);
        }

        private IQueryable<ChatMessage> GetHisMessagesToMe(int myId, int hisId)
        {
            return _context.ChatMessage.Where(x => x.AuthorId == hisId && x.RecipientId == myId);
        }

        public IEnumerable<MessageModel> GetMessagesBetweenUsers(int myId, int hisId, PagingInfo pagingInfo)
        {
            return GetMyMessagesToHim(myId, hisId)
                .Union(GetHisMessagesToMe(myId, hisId))
                .Select(x => new MessageModel
                {
                    ID = x.Id,
                    Text = x.Text,
                    IsMine = x.AuthorId == myId,
                    IsRead = x.MessageStatusId == (int)Enums.MessageStatus.Read,
                    CreateDate = x.CreateDate
                })
                .OrderByDescending(x => x.CreateDate)
                .Skip(pagingInfo.CurrentPage * pagingInfo.ItemsPerPage)
                .Take(pagingInfo.ItemsPerPage)
                .OrderBy(x=>x.CreateDate);
        }

        public MessageModel CreateMessage(int authorId, int recipientId, string text)
        {
            var message = new ChatMessage
            {
                AuthorId = authorId,
                CreateDate = DateTime.Now,
                RecipientId = recipientId,
                Text = text,
                MessageStatusId = (int)Enums.MessageStatus.Sent
            };

            _context.ChatMessage.Add(message);

            _context.SaveChanges();

            return ObjectMapperManager.DefaultInstance.GetMapper<ChatMessage, MessageModel>().Map(message);
        }

        public int GetCount(int myId, int hisId)
        {
            return GetMyMessagesToHim(myId, hisId)
                .Union(GetHisMessagesToMe(myId, hisId)).Count();
        }

        public IEnumerable<MessageModel> MarkAsRead(int readerId, int opponentId)
        {
            var messagesToMe = GetHisMessagesToMe(readerId, opponentId).Where(x => x.MessageStatusId == (int)Enums.MessageStatus.Sent);

            foreach (var mes in messagesToMe)
            {
                mes.MessageStatusId = (int)Enums.MessageStatus.Read;
            }

            _context.SaveChanges();

            return messagesToMe.Select(x => new MessageModel
            {
                CreateDate = x.CreateDate,
                IsRead = true,
                IsMine = false,
                Text = x.Text,
                ID = x.Id
            });
        }
    }
}
