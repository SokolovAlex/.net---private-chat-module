using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using App.BLL.Abstract;
using App.BLL.Common.Concrete.Helpers;
using App.BLL.Common.Models.User2UserChat;
using App.BLL.Concrete;
using App.BLL.Concrete.Helpers;
using Autofac;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using ServiceStack.Redis;

namespace User2UserChat.Web.Hubs
{
    [HubName("chatHub")]
    public class ChatHub : Hub //: PersistentConnection 
    {
        private IUserHelper userHelper { get; set; }
        private IMessageHelper messageHelper { get; set; }

        public ChatHub()
        {
            userHelper = IoC.Instance.Resolve<IUserHelper>();
            messageHelper = IoC.Instance.Resolve<IMessageHelper>();
        }

        public void Register(string userId, string opponentId)
        {
            var redisHelper = IoC.Instance.Resolve<ISessionRedisHelper>();
            ((PrivateChatSessionRedisHelper)redisHelper).Add(userId, opponentId, Context.ConnectionId);
            ((PrivateChatSessionRedisHelper)redisHelper).Dispose();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var redisHelper = IoC.Instance.Resolve<ISessionRedisHelper>();

            ((PrivateChatSessionRedisHelper)redisHelper).RemoveByConnection(Context.ConnectionId);

            ((PrivateChatSessionRedisHelper)redisHelper).Dispose();

            return base.OnDisconnected(stopCalled);
        }

        public void SendMessage(MessageMsMq msg)
        {
            string authorId = msg.AuthorId;
            string recipientId = msg.RecipientId;
            string clientMessageId = msg.Id;

            var redisHelper = IoC.Instance.Resolve<ISessionRedisHelper>();

            var connections = ((PrivateChatSessionRedisHelper)redisHelper).Get(authorId, recipientId);

            if (connections != null && connections.Any())
            {
                Clients.Clients(connections).ReceiveMessage(msg.Text, true, msg.DisplayDate, clientMessageId);
            }

            var theirConnections = ((PrivateChatSessionRedisHelper)redisHelper).Get(recipientId, authorId);
            if (theirConnections == null || !theirConnections.Any()) return;

            Clients.Clients(theirConnections).ReceiveMessage(msg.Text, false, msg.DisplayDate, clientMessageId);

            ((PrivateChatSessionRedisHelper)redisHelper).Dispose();
        }

        public void SaveMessage(string text, string authorId, string recipientId, string clientMessageId)
        {
            var msmqHelper = new MsMqFactoryHelper<MessageMsMq>("ChatMessages");
            var msg = new MessageMsMq
            {
                CreateDate = DateTime.Now,
                AuthorId = authorId,
                RecipientId = recipientId,
                Text = text,
                Id = clientMessageId
            };

            msmqHelper.PushMessage(msg);

            var users = userHelper.DAL_Repository.GetChatUsersByHashes(authorId, recipientId);
            messageHelper.Repository.CreateMessage(users.Sender.ID, users.Recipient.ID, msg.Text);

            //Clients.Clients(connections).MessageSaved(clientMessageId, msg.DisplayDate);
        }

        public void ReadMessages(string authorId, string recipientId)
        {
            var users = userHelper.DAL_Repository.GetChatUsersByHashes(authorId, recipientId);

            var msgs = messageHelper.Repository.MarkAsRead(users.Sender.ID, users.Recipient.ID);

            var redisHelper = IoC.Instance.Resolve<ISessionRedisHelper>();

            var connections = ((PrivateChatSessionRedisHelper)redisHelper).Get(authorId, recipientId);

            if (connections == null || !connections.Any()) return;

            Clients.Clients(connections).RecipientReadMessages(msgs.ToList());

            ((PrivateChatSessionRedisHelper)redisHelper).Dispose();
        }
    }

  
}