using Core.Enums;
using Dal.Repositories.IRepositories;
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
        [Auth(new[] { UserRole.All })]
        public ActionResult ShowUserList()
        {
            var rep = new UserRepository();
            var user = rep.GetAll().FirstOrDefault();
            var users = rep.GetAll().Where(x=>x.Id != user.Id);

            return View(users);
        }

        // GET: PrivateChat
        public ActionResult StartChatWith(int id)
        {
            return View();
        }

    }
}