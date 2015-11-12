using Core.Enums;
using Core.Models.User;
using Dal.Repositories.IRepositories;
using Dal.Repositories.Repositories;
using Services.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Core.Components;

namespace PrivateChat.Web.Filters
{
    public class Auth : AuthorizeAttribute
    {
        private const string CoockieKey = "ApplicationUser";

        private UserRole[] UserRoles { get; set; }

        private AuthServise authService { get; set; }

        public Auth(UserRole[] roles ) {
            UserRoles = roles;
            authService = new AuthServise();
        }

        public Auth()
        {
            UserRoles = new[] { UserRole.All };
            authService = new AuthServise();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var coockie = filterContext.HttpContext.Request.Cookies.Get(CoockieKey);

            UserModel user = null;
            if (coockie != null)
            {
                //user = authService.VerifyUser(coockie.Value, UserRoles);
                if (CurrentUser.Info == null)
                {
                    var rep = IoC.Instance.Resolve<IUserRepository>();
                    Guid guid;
                    var check = Guid.TryParse(coockie.Value, out guid);
                    if (check) {
                        user = rep.GetByHash(guid);
                        CurrentUser.Info = new UserSessionModel {
                            UserModel = user
                        };
                    }
                }
                else {
                    user = CurrentUser.Info.UserModel;
                }
            }

            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = filterContext.HttpContext.Request.Url }));
            }
        }
    }
}