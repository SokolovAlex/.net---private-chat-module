using AutoMapper;
using Core.Models;
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
        public UserRepository() {
            Mapper.CreateMap<User, UserModel>();
        }

        protected override DbSet<User> GetTable() {
            return context.Users;
        }

        public UserModel GetByHash(string hashId)
        {
            return Mapper.Map<UserModel>(
                GetTable().FirstOrDefault(x=>x.HashId == hashId));
        }
    }
}
