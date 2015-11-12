using Core.Enums;
using Core.Models;
using Core.Models.User;
using Dal.Repositories.IRepositories;
using Dal.Repositories.Repositories;
using DevOne.Security.Cryptography.BCrypt;
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

        private const string CoockieKey = "ApplicationUser";

        public void InitializeCurrentUser(UserModel user)
        {
            var userData = new UserSessionModel { UserModel = user };
            CurrentUser.Info = userData;
        }

        public AppResult VerifyUser(UserModel user, string pswd) {
            if (user == null) {
                return new AppResult {
                    isError = true,
                    Message = "No User"
                };
            }

            var isValidPassword = BCryptHelper.CheckPassword(pswd, user.Password);

            if (!isValidPassword) {
                return new AppResult
                {
                    isError = true,
                    Message = "Password incorrect"
                };
            }

            return new AppResult
            {
                isError = false,
                Message = "Password correct"
            };
        }

        public void SaveToCookie()
        {
            if (CurrentUser.Info == null || CurrentUser.Info.UserModel.HashId == null) {
                return;
            }

            //encrypt the ticket and add it to a cookie
            var cookie = new HttpCookie(
                CoockieKey,
                CurrentUser.Info.UserModel.HashId.ToString());

            cookie.Expires = DateTime.Now.AddHours(1);

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

        public UserModel RegisterUser(UserModel model, string password) {
            var userRepository = new UserRepository();
            var salt = BCryptHelper.GenerateSalt();
            var passwordHash = BCryptHelper.HashPassword(password, salt);

            model.Salt = salt;
            model.Password = passwordHash;

            userRepository.Save(model);
            userRepository.Commit();
          
            return model;
        }

        public UserModel Login(UserModel model)
        {
            InitializeCurrentUser(model);
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
