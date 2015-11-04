using Bll.Providers;
using Core.Enums;
using Core.Models;
using Core.Models.PrivateChat;
using Dal.Repositories.IRepositories;
using Dal.Repositories.PrivatChat;
using Dal.Repositories.Repositories;
using PrivateChat.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateChat.Web.Controllers
{
    public class PrivateChatController : Controller
    {
        // GET: PrivateChat
        public ActionResult Index()
        {
            return View();
        }

        // GET: PrivateChat
        [Auth]
        public ActionResult ShowUserList()
        {
            var rep = new UserRepository();
            var curretId = CurrentUser.Info.UserModel.Id;
            var users = rep.GetAllRecipientsFor(curretId);
            return View(users);
        }

        // GET: PrivateChat
        [Auth]
        public ActionResult StartChatWith(Guid id)
        {
            var messageRep = new MessageRepository();
            var rep = new UserRepository();
            var currentUser = CurrentUser.Info.UserModel;
            var recipient = rep.GetByHash(id);
            var pageSize = Int32.Parse(SettingsProvider.Instance.Settings[Settings.ChatPageSize].Value);

            var model = new PrivateChatRoom
            {
                Author = currentUser,
                Recipient = recipient,
                MessagesAmount = messageRep.GetCount(currentUser.Id, recipient.Id),
                Messages = messageRep.GetMessagesBetweenUsers(currentUser.Id, recipient.Id, new PagingInfo
                {
                    CurrentPage = 0,
                    ItemsPerPage = pageSize
                })
            };
            return View("ChatView", model);
        }

        public ActionResult GetMessagesWith(Guid id, int page = 1, int itemPerPage = 0)
        {
            var messageRep = new MessageRepository();
            var rep = new UserRepository();
            var currentUser = CurrentUser.Info.UserModel;
            var recipient = rep.GetByHash(id);
            var defaultPageSize = Int32.Parse(SettingsProvider.Instance.Settings[Settings.ChatPageSize].Value);

            var model = new PrivateChatRoom
            {
                Messages = messageRep.GetMessagesBetweenUsers(currentUser.Id, recipient.Id, new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = itemPerPage == 0 ? defaultPageSize : itemPerPage,
                })
            };

            return new JsonResult
            {
                Data = model.MessagesJson,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}