using Core.Enums;
using Core.Models;
using Dal.Repositories.IRepositories;
using Services.Providers.IProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Providers
{
    public class AuthServise: IAuthServise
    {
        private IUserRepository UserRepository { get; set; }

        public AuthServise() {
            UserRepository = new UserRepository();
        }

        public UserModel VerifyHash(string hash, UserRole[] roles) {
            var user = UserRepository.GetByHash(hash);

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
    }
}
