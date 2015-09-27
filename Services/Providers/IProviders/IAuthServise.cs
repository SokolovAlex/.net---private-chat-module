using Core.Enums;
using Core.Models;
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
    }
}
