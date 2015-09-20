using Dal.DbEntities;
using Ssibir.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repositories.IRepositories
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        protected override DbSet<User> GetTable() {
            return context.Users;
        }
    }
}
