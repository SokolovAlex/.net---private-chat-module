using Core.Enums;
using Core.Models;
using Services.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PrivateChat.Web.Filters
{
    public class Auth : System.Web.Mvc.AuthorizeAttribute
    {
        private const string CoockieKey = "ApplicationUser";

        private UserRole[] UserRoles { get; set; }

        private AuthServise authService { get; set; }

        public Auth(UserRole[] roles) {
            UserRoles = roles;
            authService = new AuthServise();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var userIdentity = filterContext.RequestContext.HttpContext.User;
            var coockie = filterContext.HttpContext.Request.Cookies.Get(CoockieKey);

            UserModel user = null;
            if (coockie != null) {
                user = authService.VerifyHash(coockie.Value, UserRoles);
            }

            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = filterContext.HttpContext.Request.Url }));
                //base.HandleUnauthorizedRequest(filterContext);
            }
            else {
                base.OnAuthorization(filterContext);
            }

        }
    }
}