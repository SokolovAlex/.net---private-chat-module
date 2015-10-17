using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

using Autofac;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using Dal.Repositories.Repositories;
using Dal.Repositories.PrivatChat;

namespace PrivateChat.Web.Hubs
{
    [HubName("chatHub")]
    public class ChatHub : Hub //: PersistentConnection 
    {
        private IDictionary<Guid, List<ConnectionInfo>> Connections { get; set; }

        public ChatHub() {
            Connections = new Dictionary<Guid, List<ConnectionInfo>>();
        }

        public void Register(Guid userId, Guid opponentId)
        {
            var conInfo = new ConnectionInfo { With = opponentId, Connection = Context.ConnectionId };

            if (Connections.ContainsKey(userId))
            {
                var userCons = Connections[userId];
                if (userCons == null)
                {
                    userCons = new List<ConnectionInfo> { conInfo };
                }
                else
                {
                    userCons.Add(conInfo);
                }
            }
            else
            {
                Connections.Add( userId, new List<ConnectionInfo> { conInfo });
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var isFound = false;
            foreach (var userCons in Connections) {
                foreach (var con in userCons.Value) {
                    if (con.Connection == Context.ConnectionId) {
                        userCons.Value.Remove(con);
                        if (!userCons.Value.Any()) {
                            Connections.Remove(userCons.Key);
                        }
                        isFound = true;
                        break;
                    }
                }
                if (isFound) {
                    break;
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        public void SaveMessage(string text, Guid authorId, Guid recipientId, string clientMessageId)
        {
            var userRepository = new UserRepository();
            var messageRepository = new MessageRepository();

            var users = userRepository.GetChatUsersByHashes(authorId, recipientId);

            var msg =  messageRepository.CreateMessage(users.Author.Id, users.Recipient.Id, text);

            var connections = Connections[authorId].Select(x=>x.Connection).ToList();
            Clients.Clients(connections).MessageSaved(clientMessageId, msg.DisplayCreateDate, text);
        }

        public void ReadMessages(Guid authorId, Guid recipientId)
        {
            var userRepository = new UserRepository();
            var messageRepository = new MessageRepository();
            var users = userRepository.GetChatUsersByHashes(authorId, recipientId);

            var msgs = messageRepository.MarkAsRead(users.Author.Id, users.Recipient.Id);

            var connections = Connections[recipientId].Where(x=>x.With == authorId).Select(x=>x.Connection).ToList();

            if (connections == null || !connections.Any()) return;

            Clients.Clients(connections).RecipientReadMessages(msgs.ToList());
        }
    }

    public class ConnectionInfo {
        public Guid With { get; set; }
        public string Connection { get; set; }
    }
}