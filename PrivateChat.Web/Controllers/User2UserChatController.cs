using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.BLL.Abstract;
using App.BLL.Common.Concrete.Helpers;
using App.BLL.Common.Models.Helper;
using App.BLL.Common.Models.User;
using App.BLL.Common.Models.User2UserChat;
using App.BLL.Concrete;
using App.BLL.Concrete.Helpers;
using App.BLL.Filters;
using App.DAL.DataBaseModel;
using Autofac;
using Newtonsoft.Json;

namespace User2UserChat.Web.Controllers
{
    public class User2UserChatController : Controller
    {
        private IUserHelper userHelper { get; set; }
        private IMessageHelper messageHelper { get; set; }

        public User2UserChatController()
        {
            userHelper = IoC.Instance.Resolve<IUserHelper>();
            messageHelper = IoC.Instance.Resolve<IMessageHelper>();
        }

        // GET: User2UserChat
        public ActionResult UserListView()
        {
            var currentUser = CurrentUser.Info.UserModel;

            var userList = userHelper.DAL_Repository.GetAllRecipientsFor(currentUser.ID);

            return View(userList);
        }

        // GET: User2UserChat
        public ActionResult UserListViewFor(string id)
        {
            var currentUser = userHelper.DAL_Repository.User_GetByHashID(id, true);
            UserSessionModel sModel = new UserSessionModel
            {
                UserModel = currentUser
            };
            CurrentUser.Info = sModel;

            var userList = userHelper.DAL_Repository.GetAllRecipientsFor(currentUser.ID);

            return View("UserListView", userList);
        }

        public ActionResult UsersList(string id)
        {
            var currentUser = userHelper.DAL_Repository.User_GetByHashID(id, true);
            UserSessionModel sModel = new UserSessionModel
            {
                UserModel = currentUser
            };
            CurrentUser.Info = sModel;

            var userList = userHelper.DAL_Repository.GetAllRecipientsFor(currentUser.ID);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(userList),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult StartChatWith(string id)
        {
            var currentUser = CurrentUser.Info.UserModel;
            var recipient = userHelper.DAL_Repository.User_GetByHashID(id, true);

            var model = new ChatViewModel
            {
                FromUser = currentUser,
                ToUser = recipient,
                MessagesAmount = messageHelper.Repository.GetCount(currentUser.ID, recipient.ID),
                Messages = messageHelper.Repository.GetMessagesBetweenUsers(currentUser.ID, recipient.ID, new PagingInfo
                {
                    CurrentPage = 0,
                    ItemsPerPage = 20
                })
            };

            return View("ChatView", model);
        }

        public ActionResult GetMessagesWith(string id, int page = 1, int itemPerPage = 20)
        {
            var currentUser = CurrentUser.Info.UserModel;
            var recipient = userHelper.DAL_Repository.User_GetByHashID(id, true);

            var model = new ChatViewModel
            {
                Messages = messageHelper.Repository.GetMessagesBetweenUsers(currentUser.ID, recipient.ID, new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = itemPerPage,
                })
            };

            return new JsonResult
            {
                Data = model.MessagesToJsonObject(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult GetMessagesBetween(string authorId, string recipientId, int page = 1, int itemPerPage = 20)
        {
            var users = userHelper.DAL_Repository.GetChatUsersByHashes(authorId, recipientId);

            var author = users.Sender;
            var recipient = users.Recipient;

            if (author == null || recipient == null)
            {
                return new EmptyResult();
            }

            var model = new ChatViewModel
            {
                Messages = messageHelper.Repository.GetMessagesBetweenUsers(author.ID, recipient.ID, new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = itemPerPage,
                })
            };

            return new JsonResult
            {
                Data = model.MessagesToJsonObject(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}