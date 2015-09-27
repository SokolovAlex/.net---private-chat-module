using Core.Models;
using Dal.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repositories.IRepositories
{
    public interface IUserRepository: IBaseRepository<User>
    {
        UserModel GetByHash(string hashId);
    }
}
