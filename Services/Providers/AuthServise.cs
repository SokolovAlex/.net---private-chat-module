using Core.Enums;
using Core.Models;
using Core.Models.User;
using Dal.Repositories.IRepositories;
using Services.Providers.IProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Services.Providers
{
    public class AuthServise: IAuthServise
    {
        public AuthServise() {
           
        }

        public void InitializeCurrentUser(UserModel user)
        {
            var userData = new UserSessionModel { UserModel = user };
            CurrentUser.Info = userData;
        }

        public UserModel VerifyHash(string hash, UserRole[] roles) {
            var userRepository = new UserRepository();
            var user = userRepository.GetByHash(hash);

            if (user == null) {
                return null;
                //throw NotFindException();
            }

            if (roles.Contains(UserRole.All) || roles.Contains(user.UserRole))
            {
                return user;
            }
            else {
                return null;
                //throw permissionException();
            }
        }

        public void SaveToCookie()
        {
            //create the authentication ticket
            var authTicket = new FormsAuthenticationTicket(
                1,
                CurrentUser.Info.UserModel.Id.ToString(),
                //user id
                DateTime.Now,
                DateTime.Now.AddMinutes(10 * 60 * 1000),
                // expiry
                true,
                //true to remember
                "",
                //roles 
                "/"
                );

            //encrypt the ticket and add it to a cookie
            var cookie = new HttpCookie(
                FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(authTicket));
            cookie.Expires = authTicket.Expiration;

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void ClearCookies()
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var response = httpContext.Response;

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
            {
                Expires = DateTime.Now.AddDays(-1) // or any other time in the past
            };

            response.Cookies.Set(cookie);

            httpContext.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
            response.Cookies.Remove(FormsAuthentication.FormsCookieName);
        }

        public UserModel RegisterUser(UserModel model) {
            var userRepository = new UserRepository();
            InitializeCurrentUser(model);
            userRepository.Save(model);
            userRepository.Commit();
            SaveToCookie();
            return model;
        }

        public void LogOut()
        {
            CurrentUser.Info = null;
            this.ClearCookies();
        }
    }
}
