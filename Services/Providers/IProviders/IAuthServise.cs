using Core.Enums;
using Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Providers.IProviders
{
    public interface IAuthServise
    {
        UserModel VerifyHash(string hash, UserRole[] roles);

        void InitializeCurrentUser(UserModel user);

        UserModel RegisterUser(UserModel model);
    }
}
